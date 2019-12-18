using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SceneManagement;


[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class CollisionSystem : JobComponentSystem
{
	EntityQuery enemyGroup;
	EntityQuery enemyBulletGroup;
	EntityQuery playerBulletGroup;

	protected override void OnCreate()
	{
		// 搜出所有帶有以下特質的Entity
		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
		enemyBulletGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyBulletTag>());
		playerBulletGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PlayerBulletTag>());
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
		float playerRadius = 0.8f;

		JobHandle jobHandle = new JobHandle();

		if (SceneManager.GetActiveScene().buildIndex != 2) // if not stage 1
		{
			// 改 stage2 BOSS & 玩家 的 Raduis
			if (Done_GameController_stage2.bossShow == true)
			{
				enemyRadius *= 3f;
				playerRadius /= 1.5f;
			}

			// 改 stage3 BOSS & 玩家 的 Raduis
			if (Done_GameController_stage3.bossShow == true)
			{
				enemyRadius *= 1.5f;
				playerRadius /= 1.5f;
			}

			// 處理敵機損血
			var jobEvB = new CollisionJob()
			{
				radius = enemyRadius * enemyRadius,
				healthType = healthType,
				translationType = translationType,
				transToTestAgainst = playerBulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
			};

			jobHandle = jobEvB.Schedule(enemyGroup, inputDependencies);

			// 處理子彈銷毀
			var jobBvE = new CollisionJob()
			{
				radius = enemyRadius * enemyRadius,
				healthType = healthType,
				translationType = translationType,
				transToTestAgainst = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
			};

			jobHandle = jobBvE.Schedule(playerBulletGroup, jobHandle);

		}

		// 處理玩家撞敵機
		NativeArray<Translation> enemyBulletPosition;
		NativeArray<Translation> enemyPosition;
		bool gameOver = false;

		enemyBulletPosition = enemyBulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob);
		enemyPosition = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob);

		// if stage 1 easier
		if (SceneManager.GetActiveScene().buildIndex == 2)
			playerRadius /= 1.5f;

		for (int i = 0; i < enemyBulletPosition.Length; i++)
		{
			float dx, dz;
			if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				dx = Done_PlayerController_stage1.playerPosition.x - enemyBulletPosition[i].Value.x;
				dz = Done_PlayerController_stage1.playerPosition.z - enemyBulletPosition[i].Value.z;
			}
			else
			{
				dx = Done_PlayerController_stage2.playerPosition.x - enemyBulletPosition[i].Value.x;
				dz = Done_PlayerController_stage2.playerPosition.z - enemyBulletPosition[i].Value.z;
			}

			if (dx * dx + dz * dz <= playerRadius)
			{
				gameOver = true;
				if (SceneManager.GetActiveScene().buildIndex == 2)
					Done_GameController_stage1.gameOver = true;
				else if (SceneManager.GetActiveScene().buildIndex == 4)
					Done_GameController_stage2.gameOver = true;
				else
					Done_GameController_stage3.gameOver = true;
			}
		}


		if (SceneManager.GetActiveScene().buildIndex != 2) // if not stage 1 detect
		{
			for (int i = 0; i < enemyPosition.Length; i++)
			{
				float dx, dz;
				if (SceneManager.GetActiveScene().buildIndex == 2)
				{
					dx = Done_PlayerController_stage1.playerPosition.x - enemyPosition[i].Value.x;
					dz = Done_PlayerController_stage1.playerPosition.z - enemyPosition[i].Value.z;
				}
				else
				{
					dx = Done_PlayerController_stage2.playerPosition.x - enemyPosition[i].Value.x;
					dz = Done_PlayerController_stage2.playerPosition.z - enemyPosition[i].Value.z;
				}

				if (dx * dx + dz * dz <= playerRadius + enemyRadius)
				{
					gameOver = true;
					if (SceneManager.GetActiveScene().buildIndex == 2)
						Done_GameController_stage1.gameOver = true;
					else if (SceneManager.GetActiveScene().buildIndex == 4)
						Done_GameController_stage2.gameOver = true;
					else
						Done_GameController_stage3.gameOver = true;
				}
			}
		}

		// If GameOver 就刪除所有的敵機 & 玩家
		if (gameOver)
		{
			Object.Destroy(GameObject.Find("Done_Player")); // Delete Player
			if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				Object.Destroy(GameObject.Find("Boy_stage1")); // Delete Boy
			}
		}

		enemyPosition.Dispose();
		enemyBulletPosition.Dispose();

		return jobHandle;
	}

	static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
	{
		float3 delta = posA - posB;
		float distanceSquare = delta.x * delta.x + delta.z * delta.z;

		return distanceSquare <= radiusSqr;
	}
}
