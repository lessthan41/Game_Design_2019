using Unity.Entities;
using UnityEngine;

// for stage 3 boss prefab
public class BossBehaviour_stage3 : MonoBehaviour, IConvertGameObjectToEntity
{
	// boss info
	public float speed;
	public float enemyHealth;

	// when boss is converted into entity
	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(MoveForward)); // move vertically
		manager.AddComponent(entity, typeof(BossMoving)); // move horizontally
	    manager.AddComponent(entity, typeof(EnemyTag)); // this is enemy
		manager.AddComponent(entity, typeof(BossTag)); // this is boss

	    MoveSpeed moveSpeed = new MoveSpeed { ValueX = speed, ValueZ = speed };
	    manager.AddComponentData(entity, moveSpeed); // set moving speed

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health); // set health value
    }
}
