using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// for all fade in fade out scene operation (fade animation)
public class SceneSwitch : MonoBehaviour
{
    // for scene fade in fade out animator
    public Animator animator;

    // do not destroy BGM when loading Stage3, and destroy at Plot4
    void Start ()
    {
        if (SceneManager.GetActiveScene().buildIndex == 5)
            DontDestroyOnLoad(GameObject.Find("Background Music"));
        if (SceneManager.GetActiveScene().buildIndex == 7)
            Destroy(GameObject.Find("Background Music"));
    }

    // set fade out trigger function
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

    // application quit
    public void exit ()
    {
        Application.Quit();
    }
}
