using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Transforms
{
	// boss moving mode (horizontally move and move direction switch)
	public class BossMovingSystem : JobComponentSystem
	{

		[BurstCompile]
		[RequireComponentTag(typeof(BossMoving))] // for those entity who has BossMoving Tag
		struct BossMovingRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		// for each who has Translation, Rotation attribute, MoveSpeed tag
		{
			public float dt;

			// do when Job is executed
			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				if (pos.Value.x >= 5f || pos.Value.x <= -5f)
				{
					speed.switchDirection(pos.Value.x); // direciton change
				}

				pos.Value.x = pos.Value.x + dt * speed.ValueX;
			}
		}

		// OnUpdate do something (decide to do job or pass)
		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var moveForwardRotationJob = new BossMovingRotation
			{
				dt = Time.deltaTime
			};

			if (SceneManager.GetActiveScene().buildIndex == 2 ||
				Done_GameController_stage2.bossShow == true ||
				Done_GameController_stage3.bossMove == true) // if boss show then do boss move else return
				return moveForwardRotationJob.Schedule(this, inputDeps);
			else
				return inputDeps;
		}
	}
}
