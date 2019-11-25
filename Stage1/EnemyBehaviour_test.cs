using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyBehaviour_test : MonoBehaviour, IConvertGameObjectToEntity
{
	// 自訂參數 (敵機飛行速度、敵機血量、生存時間)
	// public float speed;
	public GameObject shot;
	public Transform shotSpawn;
	public float enemyHealth;
	public Entity enemyBulletEntityPrefab;
	// public float enemyFireRate;

	// Code Calculate Need
	// private float nextFire;

	EntityManager manager;


	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(EnemyTag));

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health);

		// shotSpawn shots = new shotSpawn { Value = shotSpawn };
    }

	void Start()
	{
		manager = World.Active.EntityManager;
        enemyBulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
	}

	// void Update()
	// {
	// 	// if(Time.time > nextFire)
	// 	// {
	// 	// 	nextFire = Time.time + enemyFireRate;
	// 	Vector3 rotation = shotSpawn.rotation.eulerAngles;
    //     rotation.x = 0f;
	//
	// 	Debug.Log(rotation);
	//
	// 	UnitBulletECS(rotation);
	// 	// }
	// }
	//
	// void UnitBulletECS(Vector3 rotation)
    // {
    //     Vector3 tempRot = rotation;
	//
    //     NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
    //     manager.Instantiate(bulletEntityPrefab, bullets);
    //     manager.SetComponentData(bullets[0], new Translation { Value = shotSpawn.position });
    //     manager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });
	//
    //     bullets.Dispose();
    // }
}
