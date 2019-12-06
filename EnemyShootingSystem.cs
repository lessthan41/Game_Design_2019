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
// 		[ReadOnly] public ArchetypeChunkComponentType<Translation> translationType;
//
// 		[DeallocateOnJobCompletion]
// 		[ReadOnly] public NativeArray<Translation> enemyPosition;
//
// 		public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
// 		{
// 			var chunkTranslations = chunk.GetNativeArray(translationType);
// 			for (int i = 0; i < chunk.Count; i++)
// 			{
// 				Translation pos = chunkTranslations[i];
// 				Vector3 temp = new Vector3(pos.Value.x, pos.Value.y, pos.Value.z);
// 				EnemyShooting_test.enemyShotSpawn.position = temp;
// 			}
// 		}
// 	}
//
// 	protected override JobHandle OnUpdate(JobHandle inputDependencies)
// 	{
// 		var translationType = GetArchetypeChunkComponentType<Translation>(true);
// 		NativeArray<Translation> enemyPosition = enemyGroup.ToComponentDataArray<Translation>(Allocator.TempJob);
//
// 		// 處理敵機損血
// 		var jobShooting = new EnemyShootingJob()
// 		{
// 			enemyPosition = enemyPosition,
// 			translationType = translationType,
// 		};
//
// 		return jobShooting.Schedule(enemyGroup, inputDependencies);
// 	}
// }
