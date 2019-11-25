// using Unity.Burst;
// using UnityEngine;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Jobs;
// using Unity.Mathematics;
// using Unity.Transforms;
//
//
// [UpdateAfter(typeof(MoveForwardSystem))]
// [UpdateBefore(typeof(TimedDestroySystem))]
// public class EnemyShootingSystem : JobComponentSystem
// {
// 	EntityQuery enemyGroup;
//
// 	protected override void OnCreate()
// 	{
// 		// 搜出所有帶有以下特質的Entity
// 		enemyGroup = GetEntityQuery(typeof(Health), ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<EnemyTag>());
// 	}
//
// 	[BurstCompile]
// 	struct EnemyShootingJob : IJobChunk
// 	{
//         [ReadOnly]
//         public float DeltaTime;
//
// 		[ReadOnly]
//         public ArchetypeChunkComponentType<Rotation> rotationType;
//         // public EntityCommandBuffer.Concurrent ECSCommandBuffer;
//
//
// 		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
// 		{
// 			var chunkRotations = chunk.GetNativeArray(rotationType);
// 			for (int i = 0; i < chunk.Count; i++)
// 			{
// 				Vector3 rot = chunkRotations[i];
//
// 				// Rotation rot = chunkRotations[i];
//                 UnitBulletECS(rot);
// 			}
// 		}
// 	}
//
// 	protected override JobHandle OnUpdate(JobHandle inputDependencies)
// 	{
// 		var rotationType = GetArchetypeChunkComponentType<Rotation>(true);
//
// 		// 處理敵機損血
// 		var jobShooting = new EnemyShootingJob()
// 		{
// 			rotationType = rotationType,
//             // ECSCommandBuffer = ECSCommandBuffer.CreateCommandBuffer().ToConcurrent(),
//             DeltaTime = UnityEngine.Time.deltaTime
// 		};
//
// 		return jobShooting.Schedule(enemyGroup, inputDependencies);
// 	}
//
//     static void UnitBulletECS(Vector3 rotation)
//     {
//         Vector3 tempRot = rotation;
//
//         NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
//         EntityManager.Instantiate(EnemyBehaviour_test.enemyBulletEntityPrefab, bullets);
//         EntityManager.SetComponentData(bullets[0], new Translation { Value = shotSpawn.position });
//         EntityManager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });
//
//         bullets.Dispose();
//     }
// }
