using Unity.Entities;
using UnityEngine;

// for enemy in stage 2 (entity)
public class EnemyBehaviour_stage2 : MonoBehaviour, IConvertGameObjectToEntity
{
	// set enemy info
	public float speed;
	public float enemyHealth;
	public float lifeTime;

	// invicible time
	public static float invincibleTime = 0.05f;
	public static float nextCanFire = 0f;

	// add component when gameObject convert into entity
	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(MoveForward)); // move vertically
	    manager.AddComponent(entity, typeof(EnemyTag)); // this is enemy

	    MoveSpeed moveSpeed = new MoveSpeed { ValueX = speed, ValueZ = speed };
	    manager.AddComponentData(entity, moveSpeed); // set moving speed

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health); // set health value

		TimeToLive timeToLive = new TimeToLive { Value = lifeTime };
		manager.AddComponentData(entity, timeToLive); // set automatic destroy time count
    }
}
