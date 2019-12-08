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

    void Update () { }

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
}
