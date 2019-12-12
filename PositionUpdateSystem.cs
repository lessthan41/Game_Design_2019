using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateAfter(typeof(BossMovingSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class PositionUpdateSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref Translation pos, ref Health health) =>
		{
			if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
			{
				if (SceneManager.GetActiveScene().buildIndex == 2)
					EnemyShooting_stage1.SetPosition (pos.Value);
				else if (SceneManager.GetActiveScene().buildIndex == 4)
					EnemyShooting_stage2.SetPosition (pos.Value);
			}

			// if gameover delete entity
			if (GameObject.Find("Done_Player") == null)
			{
			    PostUpdateCommands.DestroyEntity(entity);
			}
		});
	}
}
