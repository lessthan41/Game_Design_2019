using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// for setting boss ranpage (health is lower than a value)
public class BossHealthSystem : ComponentSystem
{
	// OnUpdate do something
	protected override void OnUpdate()
	{
		// for each entity who has health & position attribute
		Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
		{
			// if this entity has BossTag
            if (EntityManager.HasComponent(entity, typeof(BossTag)))
            {
                // stage 2
                if (SceneManager.GetActiveScene().buildIndex == 4)
                {
					HealthBar.SetSize (health.Value / 200f); // resize HealthBar
                    if (health.Value <= 60) // ranpage
                    {
                        Done_GameController_stage2.ranpage = true;
                    }
                }

				// stage 3
                else if (SceneManager.GetActiveScene().buildIndex == 6)
                {
					HealthBar.SetSize (health.Value / 300f); // resize HealthBar
                }
			}
		});
	}
}
