using Unity.Entities;
using UnityEngine;

public class EnemyBehaviour_stage2 : MonoBehaviour, IConvertGameObjectToEntity
{

	// 自訂參數 (敵機飛行速度、敵機血量、生存時間)
	public float speed;
	public float enemyHealth;
	public float lifeTime;

	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(MoveForward));
	    manager.AddComponent(entity, typeof(EnemyTag));

	    MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
	    manager.AddComponentData(entity, moveSpeed);

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health);

		TimeToLive timeToLive = new TimeToLive { Value = lifeTime };
		manager.AddComponentData(entity, timeToLive);
    }
}
