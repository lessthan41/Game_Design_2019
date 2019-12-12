using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_stage1 : MonoBehaviour
{

    void Start ()
    {
        if (gameObject.name == "Stage1Bg_1")
        {
            transform.position = new Vector3(0.0f,-5.0f,39.95f);
        }
        else
        {
            transform.position = new Vector3(0.0f,-5.0f,4.99f);
        }
    }

    void FixedUpdate ()
    {
        Vector3 temp = new Vector3(0,0,-0.2f);
        transform.position += temp;
    }

    void Update ()
    {
        if (transform.position.z <= -28.8f)
        {
            transform.position = new Vector3(0.0f,-5.0f,39.95f);
        }
    }

}
