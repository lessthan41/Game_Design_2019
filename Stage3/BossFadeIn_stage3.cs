using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for boss fade in animation
public class BossFadeIn_stage3 : MonoBehaviour
{
    public Animator bossShowAnimation; // boss fade animator
    public float gameObjectDeleteWait;

    private bool gameStart;
    private bool bossFadeIn;

    void Start()
    {
        GetComponent<Transform>().position = new Vector3 (0f, 0f, 5.6f);
        gameStart = false;
        bossFadeIn = false;
    }

    void Update()
    {
        // check game start
        if (Done_GameController_stage3.bossShow && gameStart == false)
        {
            gameStart = true;
        }

        // boss appear
        if (gameStart == true && bossFadeIn == false)
        {
            bossFadeIn= true;
            BossShow ();
        }

        StartCoroutine( GameObjectDelete() );
    }

    // set boss show triger
    void BossShow ()
    {
        bossShowAnimation.SetTrigger ("BossFadeIn");
    }

    // delete boss animation
    IEnumerator GameObjectDelete ()
    {
        yield return new WaitForSeconds(gameObjectDeleteWait);
        Object.Destroy(GameObject.Find("BossObject"));
    }
}
