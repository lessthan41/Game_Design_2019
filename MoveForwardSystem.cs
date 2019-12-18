using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Transforms
{
	public class MoveForwardSystem : JobComponentSystem
	{
		[BurstCompile]
		[RequireComponentTag(typeof(MoveForward))]
		struct MoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		{
			public float dt;

			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				pos.Value = pos.Value + (dt * speed.Value * math.forward(rot.Value));
			}
		}

		[BurstCompile]
		[RequireComponentTag(typeof(BulletTag))]
		struct BulletMoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		{
			public float dt;

			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				pos.Value = pos.Value + (dt * speed.Value * math.forward(rot.Value));
			}
		}

		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var moveForwardRotationJob = new MoveForwardRotation
			{
				dt = Time.deltaTime
			};

			var bulletMoveForwardRotationJob = new BulletMoveForwardRotation
			{
				dt = Time.deltaTime
			};

			if (SceneManager.GetActiveScene().buildIndex == 4)
			{
				if (Done_GameController_stage2.bossShow == true)
					return bulletMoveForwardRotationJob.Schedule(this, inputDeps);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 6)
			{
				if (Done_GameController_stage3.moveForward == false)
					return bulletMoveForwardRotationJob.Schedule(this, inputDeps);
			}

			return moveForwardRotationJob.Schedule(this, inputDeps);
		}
	}
}
