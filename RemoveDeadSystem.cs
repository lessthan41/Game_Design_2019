using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RemoveDeadSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
		{
			if (health.Value <= 0)
			{
				if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
				{
					PostUpdateCommands.DestroyEntity(entity);
					Done_GameController.AddScore(10);
				}
				else if (EntityManager.HasComponent(entity, typeof(BulletTag)))
				{
					PostUpdateCommands.DestroyEntity(entity);
				}
			}
		});
	}
}
