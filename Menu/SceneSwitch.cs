using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public Animator animator;

    void Start ()
    {
        DontDestroyOnLoad(GameObject.Find("Background Music"));
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
