using Unity.Entities;
using Unity.Transforms;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealthSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref Health health, ref Translation pos) =>
		{
            if (EntityManager.HasComponent(entity, typeof(BossTag)))
            {
                // stage 2
                if (SceneManager.GetActiveScene().buildIndex == 4)
                {
					HealthBar.SetSize (health.Value / 200f);

                    // ranpage
                    if (health.Value <= 60)
                    {
                        Done_GameController_stage2.ranpage = true;
                    }
                }
			}
		});
	}
}
