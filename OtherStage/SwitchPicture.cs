using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPicture : MonoBehaviour
{
    public Sprite[] spriteArray;
    public SpriteRenderer sprite;
    public Animator animator;

    private int spriteIndex;

    void Start ()
    {
        spriteIndex = 0;
    }

    void ShowPicture ()
    {
        animator.SetTrigger ("PictureFadeIn");
    }

    void SwitchPict ()
    {
        GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex];
        spriteIndex++;
    }

    void HidePicture ()
    {
        animator.SetTrigger ("PictureFadeOut");
    }
}
