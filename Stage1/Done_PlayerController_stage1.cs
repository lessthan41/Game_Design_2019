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
	public float textureSwitchRate;
	public int spreadAmount_spawn;
	public int spreadAmount_round;
	public int fireMode;
	public Texture texture1;
	public Texture texture2;

	// Code Calculate Need
	private float nextFire;
	private float nextTextureSwitch;
	private int textureCnt;
	public static float3 playerPosition;

    EntityManager manager;
    Entity bulletEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shot, World.Active);
		nextTextureSwitch = Time.time + textureSwitchRate;
		textureCnt = 0;
    }

    void Update ()
	{
		SwitchTexture ();
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
