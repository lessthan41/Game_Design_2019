using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// player moving boundary
[System.Serializable]
public class Done_Boundary_stage1
{
	public float xMin, xMax, zMin, zMax;
}

// for player control
public class Done_PlayerController_stage1 : MonoBehaviour
{
	// Assign GameObject
	public Done_Boundary_stage1 boundary; // move boundary
	public GameObject shot;
	public Transform shotSpawn; // shooting point

	// player setting
	public float speed;
	public float fireRate;
	public float switchRate;
    public float playerHealth;
	public float textureSwitchRate;
	public int spreadAmount_spawn;
	public int spreadAmount_round;
	public int fireMode;
	public Texture texture1; // moving animation texture
	public Texture texture2;

	// Code Calculate Need
	private float nextFire;
	private float nextTextureSwitch;
	private int textureCnt;
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
		textureCnt = 0;
    }

    void Update ()
	{
		SwitchTexture (); // change texture for moving animation
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

		// get player position
		playerPosition = GetComponent<Rigidbody>().position;
	}

	// change texture for moving animation
	private void SwitchTexture ()
	{
		if (Done_GameController_stage1.gameOver == false && Time.time >= nextTextureSwitch)
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
