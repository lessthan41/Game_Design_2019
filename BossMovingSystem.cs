using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Transforms
{
	public class BossMovingSystem : JobComponentSystem
	{

		[BurstCompile]
		[RequireComponentTag(typeof(BossMoving))]
		struct BossMovingRotation : IJobForEach<Translation, Rotation, MoveSpeed>
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
			var moveForwardRotationJob = new BossMovingRotation
			{
				dt = Time.deltaTime
			};

			if (SceneManager.GetActiveScene().buildIndex == 2 ||
				Done_GameController_stage2.bossShow == true ||
				Done_GameController_stage3.bossMove == true)
				return moveForwardRotationJob.Schedule(this, inputDeps);
			else
				return inputDeps;
		}
	}
}
