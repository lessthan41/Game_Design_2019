using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


[System.Serializable]
public class Done_Boundary_stage1
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController_stage1 : MonoBehaviour
{
	// Assign GameObject
	public Done_Boundary_stage1 boundary;
	public GameObject shot;
	public Transform shotSpawn;

	// 自訂參數 (飛行速度、fireRate、血量、子彈數)
	public float speed;
	public float fireRate;
	public float switchRate;
    public float playerHealth;
	public int spreadAmount_spawn;
	public int spreadAmount_round;
	public int fireMode;

	// Code Calculate Need
	private float nextFire;
	private float nextSwitch;
	public static float3 playerPosition;

    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
    }

    void Update ()
	{
		// Switch Mode
		if (Input.GetKey("z") && Time.time > nextSwitch)
		{
			nextSwitch = Time.time + switchRate;
			fireMode = (fireMode == 3) ? 1 : (fireMode + 1);
		}

		if (Input.GetKey("space") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;

            Vector3 rotation = shotSpawn.rotation.eulerAngles;
            rotation.x = 0f;

			if (fireMode == 1)
			{
				UnitBulletECS(rotation);

			}
			else if (fireMode == 2)
			{
				SpawnBulletECS(rotation);
			}
			else
			{
				RoundBulletECS(rotation);
			}

            GetComponent<AudioSource>().Play ();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);

		// 時刻紀錄位置 & 判定是否刪除玩家、結束遊戲
		playerPosition = GetComponent<Rigidbody>().position;
	}

	void UnitBulletECS(Vector3 rotation)
    {
        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(1, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);
        manager.SetComponentData(bullets[0], new Translation { Value = shotSpawn.position });
        manager.SetComponentData(bullets[0], new Rotation { Value = Quaternion.Euler(tempRot) });

        bullets.Dispose();
    }

    void SpawnBulletECS(Vector3 rotation)
    {
        int max = spreadAmount_spawn / 2;
        int min = -max;
        max += (spreadAmount_spawn % 2 == 0) ? 0 : 1;
        int totalAmount = spreadAmount_spawn;

        Vector3 tempRot = rotation;
        int index = 0;

        NativeArray<Entity> bullets = new NativeArray<Entity>(totalAmount, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);

        for (int y = min; y < max; y++)
        {
            tempRot.y = (rotation.y + 3 * y) % 360;

            manager.SetComponentData(bullets[index], new Translation { Value = shotSpawn.position });
            manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });

            index++;
        }

        bullets.Dispose();
    }

    void RoundBulletECS(Vector3 rotation)
    {

        Vector3 tempRot = rotation;

        NativeArray<Entity> bullets = new NativeArray<Entity>(spreadAmount_round, Allocator.TempJob);
        manager.Instantiate(bulletEntityPrefab, bullets);

        for (int index = 0; index < spreadAmount_round; index++)
        {
            tempRot.y = rotation.y + 360 / spreadAmount_round * index;

            manager.SetComponentData(bullets[index], new Translation { Value = shotSpawn.position });
            manager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });

        }
        bullets.Dispose();
    }

}
