using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;

// Remove Health <= 0 entity
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class RemoveDeadSystem : ComponentSystem
{
	// Update()
	protected override void OnUpdate()
	{
		// for all entity who has Health Component and Translatioin attribute
		Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
		{
			// if health < 0
			if (health.Value <= 0)
			{
				if (EntityManager.HasComponent(entity, typeof(BossMoving)))
				{
					// player get score
					PostUpdateCommands.DestroyEntity(entity);
					Done_GameController_stage2.AddScore(9999);
					Done_GameController_stage3.AddScore(9999);
				}
				else if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
				{
					// player get score
					PostUpdateCommands.DestroyEntity(entity);
					Done_GameController_stage2.AddScore(10);
				}
				else if (EntityManager.HasComponent(entity, typeof(PlayerBulletTag)))
				{
					// bullet do nothing
					PostUpdateCommands.DestroyEntity(entity);
				}
			}

			// if gameover delete all entity
			if (GameObject.Find("Done_Player") == null)
			{
				PostUpdateCommands.DestroyEntity(entity);
			}

		});
	}
}
