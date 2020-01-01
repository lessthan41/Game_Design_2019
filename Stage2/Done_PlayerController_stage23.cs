using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

// player moving boundary
[System.Serializable]
public class Done_Boundary_stage23
{
	public float xMin, xMax, zMin, zMax;
}

// for player control
public class Done_PlayerController_stage23 : MonoBehaviour
{
	// Assign GameObject
	public Done_Boundary_stage23 boundary;
	public GameObject shot;
	public Transform shotSpawn;

	// player setting
	public float speed;
	public float fireRate;
	public float switchRate;
    public float playerHealth;
	public int spreadAmount_spawn;
	public int spreadAmount_round;
	public int fireMode;

	// moving animation texture
	public float textureSwitchRate;
	public Texture texture1;
	public Texture texture2;
	private int textureCnt;
	private float nextTextureSwitch;

	// Code Calculate Need
	private float nextFire;
	private float nextSwitch;

	// for communicating with system 
	public static float3 playerPosition;

	// for player bullet instantiate
    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
		// initialize player bullet prefab for instantiate
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);

		nextTextureSwitch = Time.time + textureSwitchRate;
    }

    void Update ()
	{
		// // Switch Mode
		// if (Input.GetKey("z") && Time.time > nextSwitch)
		// {
		// 	nextSwitch = Time.time + switchRate;
		// 	fireMode = (fireMode == 3) ? 1 : (fireMode + 1);
		// }

		if (Input.GetKeyDown("space") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;

            Vector3 rotation = shotSpawn.rotation.eulerAngles;
            rotation.x = 0f;

			if (fireMode == 1)
			{
				UnitBulletECS(rotation);
			}
			// else if (fireMode == 2)
			// {
			// 	SpawnBulletECS(rotation);
			// }
			// else
			// {
			// 	RoundBulletECS(rotation);
			// }

            GetComponent<AudioSource>().Play ();
		}
	}

	// player controller
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

		if (moveHorizontal != 0f || moveVertical != 0)
		{
			SwitchTexture ();
		}

		// get player position
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

	// change texture for moving animation
	private void SwitchTexture ()
	{
		bool check;
		if (SceneManager.GetActiveScene().buildIndex == 4)
			check = Done_GameController_stage2.gameOver;
		else if (SceneManager.GetActiveScene().buildIndex == 6)
			check = Done_GameController_stage3.gameOver;
		else
			check = false;

		if (check == false && Time.time >= nextTextureSwitch)
		{
			nextTextureSwitch = Time.time + textureSwitchRate;
			if (textureCnt == 0)
			{
				GetComponent<Renderer>().material.mainTexture = texture2;
			}
			else
			{
				GetComponent<Renderer>().material.mainTexture = texture1;
			}
			textureCnt = (textureCnt == 0) ? 1 : 0;
		}
	}

}
