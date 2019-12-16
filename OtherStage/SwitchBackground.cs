using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackground : MonoBehaviour
{
    public Texture[] bgArray;
    public Animator animator;

    void SwitchBg ()
    {
        animator.SetTrigger ("SwitchBg");
    }

    // set background texture
    void SetTexture ()
    {
        
    }
}
