using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using UnityEngine.SceneManagement;


[UpdateAfter(typeof(MoveForwardSystem))]
public class AccelerateSystem : JobComponentSystem
{
	EndSimulationEntityCommandBufferSystem buffer;
    private bool haveAccelerate;

	protected override void OnCreateManager()
	{
		buffer = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        haveAccelerate = false;
	}

	struct AccelerateJob : IJobForEachWithEntity<MoveSpeed, EnemyTag>
	{
		public EntityCommandBuffer.Concurrent commands;
		public float ratio;

		public void Execute(Entity entity, int jobIndex, ref MoveSpeed moveSpeed, [ReadOnly] ref EnemyTag enemyTag)
		{
			moveSpeed.Value *= ratio;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
        JobHandle handle = new JobHandle ();

        // stage1 最後30秒加速
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			if (Done_GameController_stage1.time <= 30 && haveAccelerate == false)
			{
				haveAccelerate = true;
				var job = new AccelerateJob
				{
					commands = buffer.CreateCommandBuffer().ToConcurrent(),
					ratio = 2.5f,
				};
				handle = job.Schedule(this, inputDeps);
			}
		}
		else
		{
			// stage2 boss 血量過低
			if (Done_GameController_stage2.ranpage == true && haveAccelerate == false)
			{
				haveAccelerate = true;
				var job = new AccelerateJob
				{
					commands = buffer.CreateCommandBuffer().ToConcurrent(),
					ratio = 2f,
				};
				handle = job.Schedule(this, inputDeps);
			}
		}

		// for restart
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
