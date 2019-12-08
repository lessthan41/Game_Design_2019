using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Transforms
{
	public class BossMovingSystem : JobComponentSystem
	{

		[BurstCompile]
		[RequireComponentTag(typeof(BossMoving))]
		struct MoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		{
			public float dt;

			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				if (pos.Value.x >= 5f || pos.Value.x <= -5f)
				{
					speed.switchDirection(pos.Value.x);
				}

				pos.Value.x = pos.Value.x + dt * speed.Value;
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var moveForwardRotationJob = new MoveForwardRotation
			{
				dt = Time.deltaTime
			};

			return moveForwardRotationJob.Schedule(this, inputDeps);
		}
	}
}
