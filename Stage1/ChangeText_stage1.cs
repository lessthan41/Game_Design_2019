using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI text change Stage 1
public class ChangeText_stage1 : MonoBehaviour
{
    public GameObject toChange1;
    public GameObject toChange2;
    public Text changingText;

    private bool haveChange1;
    private bool haveChange2;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        haveChange1 = false;
        haveChange2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (haveChange1 == false && Time.time - startTime >= 3)
        {
            haveChange1 = true;
            TextChange (toChange1);
        }

        if (haveChange2 == false && Time.time - startTime >= 6)
        {
            haveChange2 = true;
            TextChange (toChange2);
        }
    }

    public void TextChange (GameObject toChange)
    {
        changingText.text = toChange.GetComponent<Text>().text;
    }
}
