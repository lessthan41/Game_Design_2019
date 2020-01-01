using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

// For those who has TimeToLive tag count life time
[UpdateAfter(typeof(MoveForwardSystem))]
public class TimedDestroySystem : JobComponentSystem
{
	EndSimulationEntityCommandBufferSystem buffer;

	// Start()
	protected override void OnCreateManager()
	{
		buffer = World.Active.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	// for entity who have timeToLive tag
	struct CullingJob : IJobForEachWithEntity<TimeToLive>
	{
		public EntityCommandBuffer.Concurrent commands; // manager for destroy entity
		public float dt;

		// do when Job is executed
		public void Execute(Entity entity, int jobIndex, ref TimeToLive timeToLive)
		{
			timeToLive.Value -= dt;
			if (timeToLive.Value <= 0f)
				commands.DestroyEntity(jobIndex, entity);
		}
	}

	// Update()
	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		// create new job
		var job = new CullingJob
		{
			commands = buffer.CreateCommandBuffer().ToConcurrent(),
			dt = Time.deltaTime
		};

		var handle = job.Schedule(this, inputDeps); // add into job list
		buffer.AddJobHandleForProducer(handle);

		return handle;
	}
}
