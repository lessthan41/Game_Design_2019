using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SceneManagement;

// deal with all Collisions between entities and GameObject
[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class CollisionSystem : JobComponentSystem
{
	EntityQuery enemyGroup; // query for enemy
	EntityQuery enemyBulletGroup; // query for enemy bullet
	EntityQuery playerBulletGroup; // query for player bullet

	// manager on create (start)
	protected override void OnCreate()
	{
		// search for entity for each group
		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
		enemyBulletGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyBulletTag>());
		playerBulletGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<PlayerBulletTag>());
	}

	// Collision Job
	[BurstCompile]
	struct CollisionJob : IJobChunk
	{
		public float radius; // radius detect

		public ArchetypeChunkComponentType<Health> healthType; // health type for getting NativeArray
		[ReadOnly] public ArchetypeChunkComponentType<Translation> translationType; // translation type for getting NativeArray

		[DeallocateOnJobCompletion] // dispose as job complete and ReadOnly
		[ReadOnly] public NativeArray<Translation> transToTestAgainst; // get compare group (for detect Collision group)

		// do when Job is executed
		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
		{
			// get chunk NativeArray (for component data execution)
			var chunkHealths = chunk.GetNativeArray(healthType);
			var chunkTranslations = chunk.GetNativeArray(translationType);

			// for all variables in EntityQuery group
			for (int i = 0; i < chunk.Count; i++)
			{
				float damage = 0f;
				Health health = chunkHealths[i]; // get health
				Translation pos = chunkTranslations[i]; // get position

				// for all variables in compare group
				for (int j = 0; j < transToTestAgainst.Length; j++)
				{
					Translation pos2 = transToTestAgainst[j]; // to compare with
					if (CheckCollision(pos.Value, pos2.Value, radius)) // if chunk group Collide with compare group
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

	// OnUpdate do something (decide to do job or pass)
	protected override JobHandle OnUpdate(JobHandle inputDependencies)
	{
		// get types
		var healthType = GetArchetypeChunkComponentType<Health>(false);
		var translationType = GetArchetypeChunkComponentType<Translation>(true);

		// set detect radius
		float enemyRadius = 1;
		float playerRadius = 0.8f;

		JobHandle jobHandle = new JobHandle();

		if (SceneManager.GetActiveScene().buildIndex != 2) // if not stage 1
		{
			// change stage2 BOSS & player's Raduis
			if (Done_GameController_stage2.bossShow == true)
			{
				enemyRadius *= 3f;
				playerRadius /= 1.5f;
			}

			// change stage3 BOSS & player's Raduis
			if (Done_GameController_stage3.bossShow == true)
			{
				enemyRadius *= 1.5f;
				playerRadius /= 1.5f;
			}

			// deal with enemy collide with bullet
			var jobEvB = new CollisionJob()
			{
				radius = enemyRadius * enemyRadius,
				healthType = healthType,
				translationType = translationType,
				transToTestAgainst = playerBulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
			};

			jobHandle = jobEvB.Schedule(enemyGroup, inputDependencies); // add in Job list

			// deal with bullet collide with enemy
			var jobBvE = new CollisionJob()
			{
				radius = enemyRadius * enemyRadius,
				healthType = healthType,
				translationType = translationType,
				transToTestAgainst = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob)
			};

			jobHandle = jobBvE.Schedule(playerBulletGroup, jobHandle); // add in Job list

		}

		// deal with player collide with enemy or enemyBullet
		NativeArray<Translation> enemyBulletPosition;
		NativeArray<Translation> enemyPosition;
		bool gameOver = false;

		// get enemyBullet position & enemy position
		enemyBulletPosition = enemyBulletGroup.ToComponentDataArray<Translation>(Allocator.TempJob);
		enemyPosition = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob);

		// if stage 1 easier (player radius / 1.5f)
		if (SceneManager.GetActiveScene().buildIndex == 2)
			playerRadius /= 1.5f;

		// for all enemy bullet position
		for (int i = 0; i < enemyBulletPosition.Length; i++)
		{
			// calculate distance between player and enemy
			float dx, dz;
			if (SceneManager.GetActiveScene().buildIndex == 2)
			{
				dx = Done_PlayerController_stage1.playerPosition.x - enemyBulletPosition[i].Value.x;
				dz = Done_PlayerController_stage1.playerPosition.z - enemyBulletPosition[i].Value.z;
			}
			else
			{
				dx = Done_PlayerController_stage23.playerPosition.x - enemyBulletPosition[i].Value.x;
				dz = Done_PlayerController_stage23.playerPosition.z - enemyBulletPosition[i].Value.z;
			}

			if (dx * dx + dz * dz <= playerRadius) // if collide
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

		// if not stage 1 detect enemy player collision
		if (SceneManager.GetActiveScene().buildIndex != 2)
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
					dx = Done_PlayerController_stage23.playerPosition.x - enemyPosition[i].Value.x;
					dz = Done_PlayerController_stage23.playerPosition.z - enemyPosition[i].Value.z;
				}

				if (dx * dx + dz * dz <= playerRadius + enemyRadius) // if collide
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

		// If GameOver delete gameObject
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

	// collision check
	static bool CheckCollision(float3 posA, float3 posB, float radiusSqr)
	{
		float3 delta = posA - posB;
		float distanceSquare = delta.x * delta.x + delta.z * delta.z;

		return distanceSquare <= radiusSqr;
	}
}
