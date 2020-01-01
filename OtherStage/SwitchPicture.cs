using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// switch plot picture
public class SwitchPicture : MonoBehaviour
{
    public Sprite[] spriteArray; // for switch sprite
    public SpriteRenderer sprite; // get gameobject sprite container
    public Animator animator; // picture fade in fade out animator

    private int spriteIndexPic; // sprite switch index

    void Start ()
    {
        spriteIndexPic = 0;
    }

    // show picture animation trigger
    void ShowPicture ()
    {
        animator.SetTrigger ("PictureFadeIn");
    }

    // switch picture function
    void SwitchPict ()
    {
        GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndexPic];
        spriteIndexPic++;
    }

    // hide picture animation trigger
    void HidePicture ()
    {
        animator.SetTrigger ("PictureFadeOut");
    }
}
