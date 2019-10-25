using Unity.Entities;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IConvertGameObjectToEntity
{

	// 自訂參數 (敵機飛行速度、敵機血量)
	public float speed;
	public float enemyHealth;

	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(MoveForward));
	    manager.AddComponent(entity, typeof(EnemyTag));

	    MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
	    manager.AddComponentData(entity, moveSpeed);

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health);

    }

	void Start() { }
}
