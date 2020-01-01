using Unity.Entities;
using UnityEngine;

// for enemy prefab to instantiate
public class Done_Mover_Enemy_stage23 : MonoBehaviour, IConvertGameObjectToEntity
{
	// enemy info
	public float speed;
    public float lifeTime;
    public float health;

	// add component when gameObject convert into entity
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
		manager.AddComponent(entity, typeof(EnemyBulletTag)); // this is a enemy bullet
        manager.AddComponent(entity, typeof(MoveForward)); // move forward tag (move vertically)
		manager.AddComponent(entity, typeof(BulletTag)); // bullet tag (it is a bullet)

        MoveSpeed moveSpeed = new MoveSpeed { ValueX = speed, ValueZ = speed };
        manager.AddComponentData(entity, moveSpeed); // move speed tag (set bullet move speed)

        TimeToLive timeToLive = new TimeToLive { Value = lifeTime };
        manager.AddComponentData(entity, timeToLive); // time to live tag (set bullet automatic destroy count)

        Health bulletHealth = new Health { Value = health };
        manager.AddComponentData(entity, bulletHealth); // set bullet health
    }
}
