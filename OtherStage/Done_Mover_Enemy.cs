using Unity.Entities;
using UnityEngine;

public class Done_Mover_Enemy : MonoBehaviour, IConvertGameObjectToEntity
{
	// 自訂參數 (子彈飛行速度、子彈生存時間、子彈血量)
	public float speed;
    public float lifeTime;
    public float health;

    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        manager.AddComponent(entity, typeof(MoveForward));
        manager.AddComponent(entity, typeof(EnemyBulletTag));
		manager.AddComponent(entity, typeof(BulletTag));

        MoveSpeed moveSpeed = new MoveSpeed { Value = speed };
        manager.AddComponentData(entity, moveSpeed);

        TimeToLive timeToLive = new TimeToLive { Value = lifeTime };
        manager.AddComponentData(entity, timeToLive);

        Health bulletHealth = new Health { Value = health };
        manager.AddComponentData(entity, bulletHealth);

    }
}
