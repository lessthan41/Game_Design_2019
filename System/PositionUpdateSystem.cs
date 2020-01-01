using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[AlwaysUpdateSystem] // position need always update
public class PositionUpdateSystem : ComponentSystem
{
	private bool firstSet; // for stage 2 start bug
	private float prob; // update probability

	// Start()
	protected override void OnCreate()
	{
		firstSet = true;
	}

	// Update()
	protected override void OnUpdate()
	{
		prob = 0.3f; // for update randomly

		// for each entity who has Translation attribute
		Entities.ForEach((Entity entity, ref Translation pos) =>
		{
			// if entity has EnemyTag update position
			if (EntityManager.HasComponent(entity, typeof(EnemyTag)))
			{
				if (SceneManager.GetActiveScene().buildIndex == 4)
				{
					if (UnityEngine.Random.Range(0f, 1f) < prob || firstSet == true)
						EnemyShooting_stage2.SetPosition (pos.Value);

					if (EntityManager.HasComponent(entity, typeof(BossTag)))
					{
						EnemyShooting_stage2.SetPosition (pos.Value);
					}
				}
				else if (SceneManager.GetActiveScene().buildIndex == 6)
				{
					EnemyShooting_stage3.SetPosition (pos.Value);
				}
			}

			prob *= 0.8f;
			firstSet = false;
		});
	}
}
