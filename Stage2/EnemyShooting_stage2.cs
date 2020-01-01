using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// shooting bullet entity in stage 2
public class EnemyShooting_stage2 : MonoBehaviour
{
    // set shotspawn and three attack ways
    public Transform enemyShotSpawn;
    public GameObject shot;
    public Done_GameController_stage2 game;
    public float speed;
    public float EnemyFireRate1;
    public float EnemyFireRate2;
    public float EnemyFireRate3;
    public int spreadAmount_spawn; // bullet amount
    public int spreadAmount_round; // bullet amount

    // for shotSpawn communication
    public static Vector3 shotSpawnRecorder;

    // for communication
    public static bool startShooting;
    public static  bool isLaser;
    public static  bool isUnit;
    public static  bool isSpawn;
    public static  bool isRound;

    // Code Calculate Need
    private float nextFire1;
    private float nextFire2;
    private float nextFire3;
    private float rotateDegree;

    // entity manager & entity prefab for instantiate
    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        // initialize manager and bullet prefab for instantiate
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
        nextFire1 = Time.time + game.GetStartWait() + 1;
        startShooting = false;
        isLaser = false;
        isUnit = true;
        isSpawn = false;
        isRound = false;
        rotateDegree = 0f;
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
                if ((Time.time > nextFire1) || isLaser == true)
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
                    rotateDegree += 10;
                    RoundBulletECS(rotation);
                    GetComponent<AudioSource>().Play ();
                }
            }
        }
    }

    // unit shooting
    void UnitBulletECS(Vector3 rotation)
    {
        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
        manager.Instantiate(bulle tEntityPrefab, bullets);
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
