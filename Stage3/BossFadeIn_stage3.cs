using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFadeIn_stage3 : MonoBehaviour
{
    public Animator bossShowAnimation;
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

    void BossShow ()
    {
        bossShowAnimation.SetTrigger ("BossFadeIn");
    }

    IEnumerator GameObjectDelete ()
    {
        yield return new WaitForSeconds(gameObjectDeleteWait);
        Object.Destroy(GameObject.Find("BossObject"));
    }
}
