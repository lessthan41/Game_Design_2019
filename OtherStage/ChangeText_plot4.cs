using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// for ending scene change text
public class ChangeText_plot4 : MonoBehaviour
{
    // ending animation
    public Animator animator;

    // set ending trigger
    public void TextChange ()
    {
        animator.SetTrigger("end");
    }

    // when the end send back to Scene 1
    public void theEnd()
    {
        SceneManager.LoadScene(0);
    }
}
