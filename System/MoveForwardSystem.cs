using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Transforms
{
	// deal with entity who has moveForward tag
	public class MoveForwardSystem : JobComponentSystem
	{
		[BurstCompile]
		[RequireComponentTag(typeof(MoveForward))] // need moveForward tag
		struct MoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		// for all entity who have Translation, Rotation attribute, MoveSpeed tag
		{
			public float dt;

			// do when Job is executed
			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				pos.Value = pos.Value + (dt * speed.ValueZ * math.forward(rot.Value));
			}
		}

		// only for bullet move forward (when boss don't need moveForward but bullet need)
		[BurstCompile]
		[RequireComponentTag(typeof(BulletTag))] // need bullet tag
		struct BulletMoveForwardRotation : IJobForEach<Translation, Rotation, MoveSpeed>
		// for all entity who have Translation, Rotation attribute, MoveSpeed tag
		{
			public float dt;

			// do when Job is executed
			public void Execute(ref Translation pos, [ReadOnly] ref Rotation rot, [ReadOnly] ref MoveSpeed speed)
			{
				pos.Value = pos.Value + (dt * speed.ValueZ * math.forward(rot.Value));
			}
		}

		// OnUpdate do something (decide to do job or pass)
		protected override JobHandle OnUpdate(JobHandle inputDeps)
		{
			var moveForwardRotationJob = new MoveForwardRotation // create new job
			{
				dt = Time.deltaTime
			};

			var bulletMoveForwardRotationJob = new BulletMoveForwardRotation // create new job for only bullet
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
