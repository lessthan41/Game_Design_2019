using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;


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

	struct CullingJob : IJobForEachWithEntity<MoveSpeed, EnemyTag>
	{
		public EntityCommandBuffer.Concurrent commands;

		public void Execute(Entity entity, int jobIndex, ref MoveSpeed moveSpeed, [ReadOnly] ref EnemyTag enemyTag)
		{
			moveSpeed.Value *= 2.5f;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
        JobHandle handle = new JobHandle ();

        // stage1 最後30秒加速
        if (Done_GameController_stage1.time == 30 && haveAccelerate == false)
        {
            haveAccelerate = true;
            var job = new CullingJob
    		{
    			commands = buffer.CreateCommandBuffer().ToConcurrent(),
    		};
            handle = job.Schedule(this, inputDeps);
        }

		buffer.AddJobHandleForProducer(handle);

		return handle;
	}
}
