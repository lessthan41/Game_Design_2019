using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// get health bar
public class HealthBar : MonoBehaviour
{
    // healthbar show animator
    public Animator healthBarAnimator;
    public Done_GameController_stage3 game;

    // show health bar
    public static bool showHealth;
    public static float scaleForSet;

    // For code calculation
    private Transform bar;
    private bool hasShown;
    private float scale;
    private int stage;

    private void Start()
    {
        bar = transform.Find("HealthBar/Bar");
        showHealth = false;
        hasShown = false;
        scale = 1f;
        scaleForSet = 1f;
        stage = SceneManager.GetActiveScene().buildIndex / 2;
    }

    // healthbar operation
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

        if (stage == 2) // stage 2
        {
            if (scale <= 0.3f)
            {
                SetColor (Color.red);
            }
        }
        else if (stage == 3) // stage 3
        {
            if (scale <= 0.66f)
            {
                SetColor (Color.yellow);
                game.SetBossMode(2);
            }
            if (scale <= 0.33f)
            {
                SetColor (Color.red);
                game.SetBossMode(3);
            }
        }
    }

    // set healthbar scale
    public static void SetSize (float sizeNormalized)
    {
        scaleForSet = sizeNormalized;
    }

    // set health bar color
    private void SetColor (Color c)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = c;
    }
}
