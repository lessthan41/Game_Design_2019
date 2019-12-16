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
				if (EntityManager.HasComponent(entity, typeof(BossMoving)))
				{
					PostUpdateCommands.DestroyEntity(entity);
					Done_GameController_stage2.AddScore(9999);
				}
				else if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
				{
					PostUpdateCommands.DestroyEntity(entity);
					Done_GameController_stage2.AddScore(10);
				}
				else if (EntityManager.HasComponent(entity, typeof(PlayerBulletTag)))
				{
					PostUpdateCommands.DestroyEntity(entity);
				}
			}

			// if gameover delete entity
			if (GameObject.Find("Done_Player") == null)
			{
				// if (!EntityManager.HasComponent(entity, typeof(BossTag)))
					PostUpdateCommands.DestroyEntity(entity);
			}
			
		});
	}
}
