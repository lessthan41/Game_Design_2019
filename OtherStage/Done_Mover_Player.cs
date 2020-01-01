using Unity.Entities;
using UnityEngine;

// for player bullet prefab to instantiate playerbullet
public class Done_Mover_Player : MonoBehaviour, IConvertGameObjectToEntity
{
	// bullet info
	public float speed;
    public float lifeTime;
    public float health;

	// add component when gameObject convert into entity
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        manager.AddComponent(entity, typeof(MoveForward)); // move vertically
        manager.AddComponent(entity, typeof(PlayerBulletTag)); // it is a player bullet
		manager.AddComponent(entity, typeof(BulletTag)); // it is a bullet

        MoveSpeed moveSpeed = new MoveSpeed { ValueX = speed, ValueZ = speed };
        manager.AddComponentData(entity, moveSpeed); // add bullet move speed

        TimeToLive timeToLive = new TimeToLive { Value = lifeTime };
        manager.AddComponentData(entity, timeToLive); // add automatic destroy time count

        Health bulletHealth = new Health { Value = health };
        manager.AddComponentData(entity, bulletHealth); // add bullet health
    }
}
