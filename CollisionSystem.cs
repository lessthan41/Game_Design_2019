using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;


[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class CollisionSystem : JobComponentSystem
{
	EntityQuery enemyGroup;
	EntityQuery bulletGroup;

	protected override void OnCreate()
	{
		// 搜出所有帶有以下特質的Entity
		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
		bulletGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<BulletTag>());
	}

	[BurstCompile]
	struct CollisionJob : IJobChunk
	{
		public float radius;

		public ArchetypeChunkComponentType<Health> healthType;
		[ReadOnly] public ArchetypeChunkComponentType<Translation> translationType;

		[DeallocateOnJobCompletion]
		[ReadOnly] public NativeArray<Translation> transToTestAgainst;


		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
		{
			var chunkHealths = chunk.GetNativeArray(healthType);
			var chunkTranslations = chunk.GetNativeArray(translationType);
			for (int i = 0; i < chunk.Count; i++)
			{
				float damage = 0f;
				Health health = chunkHealths[i];
				Translation pos = chunkTranslations[i];
				for (int j = 0; j < transToTestAgainst.Length; j++)
				{
					Translation pos2 = transToTestAgainst[j];
					if (CheckCollision(pos.Value, pos2.Value, radius))
					{
						damage += 1;
					}
					if (damage > 0)
					{
						health.Value -= damage;
						chunkHealths[i] = health;
					}
				}
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDependencies)
	{

		var healthType = GetArchetypeChunkComponentType<Health>(false);
		var translationType = GetArchetypeChunkComponentType<Translation>(true);

		float enemyRadius = 1;
		float playerRadius = 1;

		// 處理敵機損血
		var jobEvB = new CollisionJob()
		{
			radius = enemyRadius * enemyRadius,
			healthType = healthType,
			translationType = translationType,
			transToTestAgainst = bulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
		};

		JobHandle jobHandle = jobEvB.Schedule(enemyGroup, inputDependencies);

		// 處理子彈銷毀
		var jobBvE = new CollisionJob()
		{
			radius = enemyRadius * enemyRadius,
			healthType = healthType,
			translationType = translationType,
			transToTestAgainst = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
		};

		jobHandle = jobBvE.Schedule(bulletGroup, jobHandle);

		// 處理玩家撞敵機
		NativeArray<Translation> enemyPosition =
			enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob);


		for (int i = 0; i < enemyPosition.Length; i++)
		{
			float dx = Done_PlayerController.playerPosition.x - enemyPosition[i].Value.x;
			float dz = Done_PlayerController.playerPosition.z - enemyPosition[i].Value.z;

			if (dx * dx + dz * dz <= playerRadius)
			{
				Done_GameController.gameOver = true;
			}
		}

		// If GameOver 就刪除所有的敵機 & 玩家
		if (Done_GameController.gameOver)
		{
			Object.Destroy(GameObject.Find("Done_Player")); // Delete Player
		}

		enemyPosition.Dispose();

		return jobHandle;
	}

	static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
	{
		float3 delta = posA - posB;
		float distanceSquare = delta.x * delta.x + delta.z * delta.z;

		return distanceSquare <= radiusSqr;
	}
}
