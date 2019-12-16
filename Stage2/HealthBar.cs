using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Animator healthBarAnimator;

    public static bool showHealth;
    public static float scaleForSet;

    private Transform bar;
    private bool hasShown;
    private float scale;

    private void Start()
    {
        bar = transform.Find("HealthBar/Bar");
        showHealth = false;
        hasShown = false;
        scale = 1f;
        scaleForSet = 1f;
    }

    private void Update ()
    {
        if (showHealth == true && hasShown == false)
        {
            hasShown = true;
            healthBarAnimator.SetTrigger("HealthBarSHow");
        }

        if (scale != scaleForSet)
            scale = scaleForSet;

        bar.localScale = new Vector3 (scale, 1f, 1f);

        if (scale <= 0.3f)
        {
            SetColor (Color.white);
        }
    }

    public static void SetSize (float sizeNormalized)
    {
        scaleForSet = sizeNormalized;
    }

    private void SetColor (Color c)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = c;
    }
}
