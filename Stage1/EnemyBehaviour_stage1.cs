using UnityEngine;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// for enemy (shooting boy) in stage 1 (gameobject)
public class EnemyBehaviour_stage1 : MonoBehaviour
{
	// speed
	public float boySpeedx;
	public float boySpeedz;

	// switch texture for animation
	public float textureSwitchRate;
	public Texture texture1;
	public Texture texture2;

	// Code Calculate Need
	private int textureCnt;
	private float nextTextureSwitch;
	private float speedSwitchRate;
	private float nextSpeedSwitchZ;
	private bool haveAccelerate;

	private void Start ()
	{
		textureCnt = 0;
		nextTextureSwitch = Time.time + textureSwitchRate;
		speedSwitchRate = 1.5f;
		nextSpeedSwitchZ = Time.time + speedSwitchRate;
		haveAccelerate = false;
	}

	private void Update ()
	{
		SwitchTexture (); // set moving animation
		SwitchDirection (); // switch moving direction
		Moving (); // moving
		Accelerate (); // if stage1 less than 30s then ranpage
		EnemyShooting_stage1.SetPosition (transform.Find("Shot Spawn").transform.position); // update shooting position per frame
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

	private void Moving ()
	{
		GetComponent<Transform>().position += new Vector3 (boySpeedx, 0f, boySpeedz);
	}

	private void SwitchDirection ()
	{
		if (GetComponent<Transform>().position.x >= 5f || GetComponent<Transform>().position.x <= -5f)
		{
			if (GetComponent<Transform>().position.x * boySpeedx > 0f)
			{
				boySpeedx = -boySpeedx;
			}
		}

		if (GetComponent<Transform>().position.z >= -1f || GetComponent<Transform>().position.z <= -2f)
		{
			if (Time.time >= nextSpeedSwitchZ)
			{
				nextSpeedSwitchZ = Time.time + speedSwitchRate;
				boySpeedz = -boySpeedz;
			}
		}
	}

	private void Accelerate ()
	{
		if (Done_GameController_stage1.time <= 30 && haveAccelerate == false)
		{
			haveAccelerate = true;
			boySpeedx *= 2;
			boySpeedz *= 2;
		}
	}
}
