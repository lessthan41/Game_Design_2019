﻿using Unity.Entities;
using UnityEngine;

public class BossBehaviour_stage2 : MonoBehaviour, IConvertGameObjectToEntity
{
	// 自訂參數 (敵機飛行速度、敵機血量、生存時間)
	public float speed;
	public float enemyHealth;


	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(MoveForward));
		manager.AddComponent(entity, typeof(BossMoving));
	    manager.AddComponent(entity, typeof(EnemyTag));
		manager.AddComponent(entity, typeof(BossTag));

	    MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
	    manager.AddComponentData(entity, moveSpeed);

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health);
    }
}