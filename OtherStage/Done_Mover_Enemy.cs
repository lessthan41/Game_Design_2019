﻿using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

// for enemy bullet prefab to instantiate enemybullet
public class Done_Mover_Enemy : MonoBehaviour, IConvertGameObjectToEntity
{
	// bullet info
	public float speed;
    public float lifeTime;
    public float health;

	// add component when gameObject convert into entity
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
		// stage 1 is enemy bullet & stage 3 is player bullet
		if (SceneManager.GetActiveScene().buildIndex == 2)
			manager.AddComponent(entity, typeof(EnemyBulletTag));
		else
			manager.AddComponent(entity, typeof(PlayerBulletTag));

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
