using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// for scene switch
public class SceneSwitch_stage1 : MonoBehaviour
{
    public Animator animator; // fade animator

    public void Update ()
    {
        if (Done_GameController_stage1.time <= 0 && Done_GameController_stage1.gameOver == false)
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
