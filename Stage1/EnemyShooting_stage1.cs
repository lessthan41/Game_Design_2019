using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyShooting_stage1 : MonoBehaviour
{
    public Transform enemyShotSpawn;
    public GameObject shot;
    public float speed;
    public float EnemyFireRate;
    public int spreadAmount_spawn;
    public int spreadAmount_round;

    public static Vector3 shotSpawnRecorder;
    private float nextFire;
    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
    }

    void Update()
    {
        if (enemyShotSpawn.position != shotSpawnRecorder)
        {
            enemyShotSpawn.position = shotSpawnRecorder + new Vector3 (-1f, 0f, 1f);
        }

        if (Input.GetKey("x") &&  Time.time > nextFire)
		{
            nextFire = Time.time + EnemyFireRate;
            Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
            rotation.x = 0f;
			UnitBulletECS(rotation);
            GetComponent<AudioSource>().Play ();
		}

        if (Input.GetKey("c") &&  Time.time > nextFire)
		{
            nextFire = Time.time + EnemyFireRate;
            Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
            rotation.x = 0f;
			SpawnBulletECS(rotation);
            GetComponent<AudioSource>().Play ();
		}

        if (Input.GetKey("v") &&  Time.time > nextFire)
		{
            nextFire = Time.time + EnemyFireRate;
            // enemyShotSpawn.position += new Vector3 (0f, 0f, -1f);
            Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
            rotation.x = 0f;
			RoundBulletECS(rotation);
            GetComponent<AudioSource>().Play ();
		}
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
            tempRot.y = (rotation.y + 3 * y) % 360;

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
