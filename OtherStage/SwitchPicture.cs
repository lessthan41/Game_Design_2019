using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPicture : MonoBehaviour
{
    public Sprite[] spriteArray;
    public SpriteRenderer sprite;
    public Animator animator;

    void ShowPicture ()
    {
        animator.SetTrigger ("PictureFadeIn");
    }

    void SwitchPict ()
    {

    }

    void HidePicture ()
    {
        animator.SetTrigger ("PictureFadeOut");
    }
}
