using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// fade in fade out stage 2
public class SceneSwitch_stage2 : MonoBehaviour
{
    public Animator animator; // fade animator
    public Done_GameController_stage2 game; // for getting game info

    public void Update ()
    {
        if (game.GetScore() >= game.GetWinScore() && game.GetGameStatus() == false)
        {
            fade();
        }
    }

    // fade out trigger
    public void fade ()
    {
        animator.SetTrigger("FadeOut");
    }

    // load next scene
    public void SceneSwitcher ()
    {
        int nextSceneID = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene (nextSceneID);
    }

    public void exit ()
    {
        Application.Quit();
    }
}
