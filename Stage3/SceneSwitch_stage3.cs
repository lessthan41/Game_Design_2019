using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// scene fade
public class SceneSwitch_stage3 : MonoBehaviour
{
    public Animator animator; // fade out animator
    public Done_GameController_stage3 game; // for getting game info

    public void Update ()
    {
        if (game.GetScore() >= game.GetWinScore() && game.GetGameStatus() == false)
        {
            fade();
        }
    }

    public void fade ()
    {
        animator.SetTrigger("FadeOut");
    }

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
