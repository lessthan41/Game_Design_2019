using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyShooting_test : MonoBehaviour
{

    public GameObject shot;
    public static Transform enemyShotSpawn;

    public float speed;

    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        // manager = World.Active.EntityManager;
        // bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
        // enemyShotSpawn = new GameObject().transform;
    }

    void Update()
    {
        // if (Input.GetKey("x"))
		// {
        //     Vector3 rotation = enemyShotSpawn.rotation.eulerAngles;
        //     rotation.x = 0f;
		// 	UnitBulletECS(rotation);
		// }
    }

    void UnitBulletECS(Vector3 rotation)
    {
        // Vector3 tempRot = rotation;
        //
        // NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
        // manager.Instantiate(bulletEntityPrefab, bullets);
        // manager.SetComponentData(bullets[0], new Translation { Value = enemyShotSpawn.position });
        // manager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });
        //
        // bullets.Dispose();
    }
}
