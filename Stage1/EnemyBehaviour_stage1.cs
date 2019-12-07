using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyBehaviour_stage1 : MonoBehaviour, IConvertGameObjectToEntity
{
	// 自訂參數 (敵機飛行速度、敵機血量、生存時間)
	public GameObject shot;
	public float stage1EnemySpeed;
	public float enemyHealth;
	public Entity enemyBulletEntityPrefab;

	private float3 enemyPosition;

	EntityManager manager;


	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(EnemyTag));
		manager.AddComponent(entity, typeof(BossMoving));

	    // Health health = new Health { Value = enemyHealth };
	    // manager.AddComponentData(entity, health);

		MoveSpeed moveSpeed = new MoveSpeed { Value = stage1EnemySpeed };
		manager.AddComponentData(entity, moveSpeed);

    }
}
