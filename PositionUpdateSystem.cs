using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;

[UpdateAfter(typeof(MoveForwardSystem))]
[UpdateAfter(typeof(BossMovingSystem))]
[UpdateBefore(typeof(TimedDestroySystem))]
public class EnemyShootingSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref Translation pos) =>
		{
			if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
			{
				EnemyShooting_stage1.SetPosition (pos.Value);
			}
		});
	}
}
