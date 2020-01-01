using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// control enemy position Instantiate enemyBullet
public class EnemyShooting_stage3 : MonoBehaviour
{
    public Transform enemyShotSpawn;
    public GameObject shot;
    public Done_GameController_stage3 game;
    public Done_Mover_Enemy_stage23 bullet;

    // fire rate
    public float speed;
    public float EnemyFireRate1;
    public float EnemyFireRate2;
    public float EnemyFireRate3;
    public float EnemyFireRate4;
    public float EnemyFireRate5;

    // fire mode switcher
    public bool isLaser;
    public bool isUnit;
    public bool isSpawn;
    public bool isRound;
    public bool isRoundRotate;
    public bool isDuo;
    public bool isTur;

    // fire amount
    public bool startShooting;
    public int spreadAmount_spawn;
    public int spreadAmount_round;
    public int spreadAmount_duo;
    public int spreadAmount_tur;

    // shotspawn position recorder
    public static Vector3 shotSpawnRecorder;

    // Code calculate needed
    private float nextFire1;
    private float nextFire2;
    private float nextFire3;
    private float nextFire4;
    private float nextFire5;
    private float rotateDegree;
    private int rotateSign;
    private int rotateSpeed;

    // entity manager & entity prefab for instantiate
    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        // initialize manager and bullet prefab for instantiate
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
        rotateDegree = 0f;
        rotateSign = 1;
        rotateSpeed = 5;
    }

    void Update()
    {
        // set shotSpawn position
        enemyShotSpawn.position = shotSpawnRecorder - new Vector3 (0f, 0f, 1f);

        // control shooting types
        if (startShooting == true)
        {
            if (isUnit)
            {
                if (Time.time > nextFire1 || isLaser == true)
                {
                    nextFire1 = Time.time + EnemyFireRate1 * UnityEngine.Random.Range(0.25f, 1f);
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    UnitBulletECS(rotation);
                    GetComponent<AudioSource>().Play ();
                }
            }

            if (isSpawn)
            {
                if (Time.time > nextFire2)
                {
                    nextFire2 = Time.time + EnemyFireRate2;
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    SpawnBulletECS(rotation);
                    GetComponent<AudioSource>().Play ();
                }
            }

            if (isRound)
            {
                if (Time.time > nextFire3)
                {
                    nextFire3 = Time.time + EnemyFireRate3;
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    rotation.y += rotateDegree;
                    rotateDegree += (rotateSign * rotateSpeed); // turning
                    RoundBulletECS(rotation, new Vector3 (0,0,0), spreadAmount_round);
                    GetComponent<AudioSource>().Play ();
                }
            }

            if (isDuo)
            {
                if (Time.time > nextFire4)
                {
                    nextFire4 = Time.time + EnemyFireRate4;
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    rotation.y += rotateDegree;
                    rotateDegree += 5; // turning
                    StartCoroutine(DuoRoundBulletECS(rotation));
                    GetComponent<AudioSource>().Play ();
                }
            }

            if (isTur)
            {
                if (Time.time > nextFire5)
                {
                    nextFire5 = Time.time + EnemyFireRate5;
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    rotation.y += rotateDegree;
                    rotateDegree += 5; // turning
                    StartCoroutine(TurbineBulletECS(rotation));
                    GetComponent<AudioSource>().Play ();
                }
            }
        }
    }

    public void SetFireRate (string str, float rate)
    {
        // unit
        if (str == "unit")
            EnemyFireRate1 = rate;
        // spawn
        else if (str == "spawn")
            EnemyFireRate2 = rate;
        // round
        else if (str == "round")
            EnemyFireRate3 = rate;
        // duo
        else if (str == "duo")
            EnemyFireRate4 = rate;
        // tur
        else if (str == "tur")
            EnemyFireRate5 = rate;
    }

    public void SetSpawnAmount (string str, int num)
    {
        // spawn
        if (str == "spawn")
            spreadAmount_spawn = num;
        // round
        else if (str == "round")
            spreadAmount_round = num;
        // duo
        else if (str == "duo")
            spreadAmount_duo = num;
        // tur
        else if (str == "tur")
            spreadAmount_tur = num;
    }

    public void SetFireOrNot (string str, bool toSet)
    {
        // Unit
        if (str == "unit")
            isUnit = toSet;
        // spawn
        else if (str == "spawn")
            isSpawn = toSet;
        // round
        else if (str == "round")
            isRound = toSet;
        else if (str == "laser")
        {
            isUnit = toSet;
            isLaser = toSet;
        }
        // duel round
        else if (str == "duo")
        {
            isDuo = toSet;
        }
        // tur round
        else if (str == "tur")
        {
            isTur = toSet;
        }
    }

    public void SetStartShooting (bool toSet)
    {
        startShooting = toSet;
    }

    public void SetRoundDirection ()
    {
        rotateSign *= -1;
    }

    public float3 GetBossPosition ()
    {
        return GetComponent<Transform>().position;
    }

    public void SetRotateSpeed (int inp)
    {
        rotateSpeed = inp;
    }

    // unit shooting
    void UnitBulletECS(Vector3 rotation)
    {
        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);
        manager.SetComponentData(bullets[0], new Translation { Value = enemyShotSpawn.position });
        manager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });

        bullets.Dispose();
    }

    // spawn shooting
    void SpawnBulletECS(Vector3 rotation)
    {
        int max = spreadAmount_spawn / 2;
        int min = -max;
        max += (spreadAmount_spawn % 2 == 0) ? 0 : 1;
        int totalAmount = spreadAmount_spawn;

        Vector3 tempRot = rotation;
        int index = 0;

        NativeArray<Entity> bullets = new NativeArray<Entity>(totalAmount, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);

        for (int y = min; y < max; y++)
        {
            tempRot.y = (rotation.y + 20 * y) % 360;

            manager.SetComponentData(bullets[index], new Translation { Value = enemyShotSpawn.position });
            manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });

            index++;
        }

        bullets.Dispose();
    }

    // round shooting
    void RoundBulletECS(Vector3 rotation, Vector3 shift, int spreadAmt) // def para should be const
    {
        Vector3 tempRot = rotation;
        NativeArray<Entity> bullets = new NativeArray<Entity>(spreadAmt, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);

        for (int index = 0; index < spreadAmt; index++)
        {
            tempRot.y = rotation.y + 360 / spreadAmt * index;
            // Add shift to the shooting position
            manager.SetComponentData(bullets[index], new Translation { Value = enemyShotSpawn.position + shift });
            manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });
        }
        bullets.Dispose();
    }

    // duo shooting
    IEnumerator DuoRoundBulletECS(Vector3 rotation)
    {
        // first wave
        RoundBulletECS(rotation, new Vector3 (0, 0, 0), spreadAmount_duo);
        // waitTime
        float waitTime = 0.25f;
        yield return new WaitForSeconds(waitTime);
        // Second Wave
        float radius = bullet.speed * waitTime;
        Vector3[] next = {
                            new Vector3 ( radius, 0, 0), new Vector3 ( radius / 1.414f, 0,  radius / 1.414f),
                            new Vector3 ( 0, 0, radius), new Vector3 ( radius / 1.414f, 0, -radius / 1.414f),
                            new Vector3 (0, 0, -radius), new Vector3 (-radius / 1.414f, 0, -radius / 1.414f),
                            new Vector3 (-radius, 0, 0), new Vector3 (-radius / 1.414f, 0,  radius / 1.414f)
                         };
        for (int dot = 0; dot < next.Length; dot++)
        {
            RoundBulletECS(rotation, next[dot], spreadAmount_duo);
        }
    }

    // turbine shooting
    IEnumerator TurbineBulletECS(Vector3 rotation)
    {
        float waitTime = 0.2f;
        // Second Wave
        float radius = 2.0f;
        Vector3[] next = {
                            new Vector3 ( radius, 0, 0), new Vector3 ( radius / 1.414f, 0,  radius / 1.414f),
                            new Vector3 ( 0, 0, radius), new Vector3 (-radius / 1.414f, 0,  radius / 1.414f),
                            new Vector3 (-radius, 0, 0), new Vector3 (-radius / 1.414f, 0, -radius / 1.414f),
                            new Vector3 (0, 0, -radius), new Vector3 ( radius / 1.414f, 0, -radius / 1.414f)
                         };
        float times = 1.0f;
        for (int wave = 1; wave <= 2; wave++) // records waves only
        {
            int dot = (wave == 1) ? 0 : 1;
            for (; dot < 8; dot += 2)
            {
                times += 0.05f;
                next[dot] = times * next[dot];
                RoundBulletECS(rotation, next[dot], spreadAmount_tur);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    public static void SetPosition (Vector3 pos)
    {
        shotSpawnRecorder = pos + new Vector3 (0f, 0f, 1f);
    }
}
