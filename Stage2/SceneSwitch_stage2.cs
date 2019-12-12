using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch_stage2 : MonoBehaviour
{
    public Animator animator;

    public void Update ()
    {
        if (Done_GameController_stage2.score >= Done_GameController_stage2.WIN_SCORE && Done_GameController_stage2.gameOver == false)
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
