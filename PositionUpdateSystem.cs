using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// [UpdateAfter(typeof(MoveForwardSystem))]
// [UpdateAfter(typeof(BossMovingSystem))]
// [UpdateBefore(typeof(MoveForwardSystem))]
[AlwaysUpdateSystem]
public class PositionUpdateSystem : ComponentSystem
{
	private bool firstSet;
	private float prob;

	protected override void OnCreate()
	{
		firstSet = true;
	}

	protected override void OnUpdate()
	{
		prob = 0.3f;

		Entities.ForEach((Entity entity, ref Translation pos) =>
		{
			if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
			{
				if (SceneManager.GetActiveScene().buildIndex == 2)
				{
					EnemyShooting_stage1.SetPosition (pos.Value);
				}
				else if (SceneManager.GetActiveScene().buildIndex == 4)
				{
					if (UnityEngine.Random.Range(0f, 1f) < prob || firstSet == true)
						EnemyShooting_stage2.SetPosition (pos.Value);

					if (EntityManager.HasComponent(entity, typeof(BossTag)))
					{
						EnemyShooting_stage2.SetPosition (pos.Value);
					}
				}
			}

			prob *= 0.8f;
			firstSet = false;
		});
	}
}
