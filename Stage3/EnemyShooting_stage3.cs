using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyShooting_stage3 : MonoBehaviour
{
    public Transform enemyShotSpawn;
    public GameObject shot;
    public Done_GameController_stage3 game;

    public float speed;
    public float EnemyFireRate1;
    public float EnemyFireRate2;
    public float EnemyFireRate3;
    public bool isLaser;
    public bool isUnit;
    public bool isSpawn;
    public bool isRound;
    public bool startShooting;
    public int spreadAmount_spawn;
    public int spreadAmount_round;

    public static Vector3 shotSpawnRecorder;

    private float nextFire1;
    private float nextFire2;
    private float nextFire3;
    private float rotateDegree;

    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
        nextFire1 = Time.time + game.GetStartWait () + 1;
        rotateDegree = 0f;
    }

    void Update()
    {
        enemyShotSpawn.position = shotSpawnRecorder - new Vector3 (0f, 0f, 1f);

        if (startShooting == true)
        {
            if (isUnit)
            {
                if ((Time.time > nextFire1 && Done_GameController_stage2.gameOver == false) || isLaser == true)
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
                if (Time.time > nextFire2 && Done_GameController_stage1.gameOver == false)
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
                if (Time.time > nextFire3 && Done_GameController_stage1.gameOver == false)
                {
                    nextFire3 = Time.time + EnemyFireRate3;
                    Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
                    rotation.x = 0f;
                    rotation.y += rotateDegree;
                    rotateDegree += 5; // turning
                    RoundBulletECS(rotation);
                    GetComponent<AudioSource>().Play ();
                }
            }
        }
    }

    public void SetFireRate (int which, float rate)
    {
        // unit
        if (which == 1)
            EnemyFireRate1 = rate;
        // spawn
        else if (which == 2)
            EnemyFireRate2 = rate;
        // round
        else
            EnemyFireRate3 = rate;
    }

    public void SetSpawnAmount (int which, int num)
    {
        // spawn
        if (which == 2)
            spreadAmount_spawn = num;
        // round
        else
            spreadAmount_round = num;
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
    }

    public void SetStartShooting (bool toSet)
    {
        startShooting = toSet;
    }

    void UnitBulletECS(Vector3 rotation)
    {
        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);
        manager.SetComponentData(bullets[0], new Translation { Value = enemyShotSpawn.position });
        manager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });

        bullets.Dispose();
    }

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

    void RoundBulletECS(Vector3 rotation)
    {
        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(spreadAmount_round, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);

        for (int index = 0; index < spreadAmount_round; index++)
        {
            tempRot.y = rotation.y + 360 / spreadAmount_round * index;

            manager.SetComponentData(bullets[index], new Translation { Value = enemyShotSpawn.position });
            manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });
        }

        bullets.Dispose();
    }

    public static void SetPosition (Vector3 pos)
    {
        shotSpawnRecorder = pos;
    }
}
