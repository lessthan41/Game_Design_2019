using Unity.Entities;
using UnityEngine;

public class EnemyBehaviour_test : MonoBehaviour, IConvertGameObjectToEntity
{

	// 自訂參數 (敵機飛行速度、敵機血量、生存時間)
	// public float speed;
	public float enemyHealth;

	public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
	{
	    manager.AddComponent(entity, typeof(EnemyTag));

	    Health health = new Health { Value = enemyHealth };
	    manager.AddComponentData(entity, health);
    }

	void Start() { }
}
