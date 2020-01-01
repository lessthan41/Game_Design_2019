using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using UnityEngine.SceneManagement;

// for boss ranpage move speed accelerate
// [UpdateAfter(typeof(MoveForwardSystem))]
public class AccelerateSystem : JobComponentSystem
{
	EndSimulationEntityCommandBufferSystem buffer;
    private bool haveAccelerate; // check boss have accelerate or not

	// manager on create (start)
	protected override void OnCreateManager()
	{
		buffer = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        haveAccelerate = false;
	}

	// accelerate Job
	struct AccelerateJob : IJobForEachWithEntity<MoveSpeed, EnemyTag> // for all entity who have MoveSpeed and EnemyTag tag
	{
		public EntityCommandBuffer.Concurrent commands; // manager
		public float ratio; // accelerate ratio

		// do when Job is executed
		public void Execute(Entity entity, int jobIndex, ref MoveSpeed moveSpeed, [ReadOnly] ref EnemyTag enemyTag)
		{
			moveSpeed.ValueX *= ratio;
			moveSpeed.ValueZ *= ratio;
		}
	}

	// OnUpdate do something (decide to do job or pass)
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
        JobHandle handle = new JobHandle ();

        // // stage1 last 30 second
		// if (SceneManager.GetActiveScene().buildIndex == 2)
		// {
		// 	if (Done_GameController_stage1.time <= 30 && haveAccelerate == false)
		// 	{
		// 		haveAccelerate = true;
		// 		var job = new AccelerateJob
		// 		{
		// 			commands = buffer.CreateCommandBuffer().ToConcurrent(),
		// 			ratio = 2.5f,
		// 		};
		// 		handle = job.Schedule(this, inputDeps);
		// 	}
		// }

		// stage2 boss health is low
		if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			// if boss is ranpage & have not accelerate
			if (Done_GameController_stage2.ranpage == true && haveAccelerate == false)
			{
				haveAccelerate = true;
				var job = new AccelerateJob // create a new job
				{
					commands = buffer.CreateCommandBuffer().ToConcurrent(),
					ratio = 2f,
				};

				handle = job.Schedule(this, inputDeps); // add into handle list
			}
		}

		// stage 3 boss change direciton
		else if (SceneManager.GetActiveScene().buildIndex == 6)
		{
			// if boss need direction change
			if (Done_GameController_stage3.bossDirectionChange == true)
			{
				Done_GameController_stage3.bossDirectionChange = false;

				var job = new AccelerateJob // assign a new job
				{
					commands = buffer.CreateCommandBuffer().ToConcurrent(),
					ratio = -1f,
				};

				handle = job.Schedule(this, inputDeps);
			}

			// if boss need speed change
			if (Done_GameController_stage3.bossSpeedChange == true)
			{
				Done_GameController_stage3.bossSpeedChange = false;

				var job = new AccelerateJob // assign a new job
				{
					commands = buffer.CreateCommandBuffer().ToConcurrent(),
					ratio = Done_GameController_stage3.bossSpeedRate,
				};

				handle = job.Schedule(this, inputDeps); // add into job list
			}
		}

		// for restart renew haveAccelerate
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			if (Done_GameController_stage1.time > 30 && haveAccelerate == true)
				haveAccelerate = false;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			if (Done_GameController_stage2.ranpage == false && haveAccelerate == true)
				haveAccelerate = false;
		}

		buffer.AddJobHandleForProducer(handle);

		return handle;
	}
}
