using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;

// control stage 1
public class Done_GameController_stage1 : MonoBehaviour
{
    public GameObject hazards; // hazard container
    public Vector3 spawnValues; // enemy instantiate point

    public int GameTime; // game time
    public float GameStartWait; // game start wait

    public float checkWait; // check gameOver every few seconds
    public static float startWait; // game start wait for communicate with system
    public static int time; // game time for communicate with system

    // UI Text
    public Text timeText;
    public Text restartText;
    public Text gameOverText;

    // status variable
    private float timeRecorder;
    public static bool gameOver;
    private bool restart;

    EntityManager manager; // entity instantiate manager
    Entity enemyEntityPrefab; // put enemy prefab

    void Start()
    {
        // initialize enemy prefab for instantiate
        manager = World.Active.EntityManager;
        enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards, World.Active);

        gameOver = false;
        restart = false;
        time = GameTime;
        startWait = GameStartWait;
        restartText.text = "";
        gameOverText.text = "";
        timeRecorder = Time.time + startWait;
        UpdateScore();
        StartCoroutine(SpawnWaves()); // start hazard wave
    }

    void Update()
    {
        // update time
        if (Time.time >= timeRecorder + 1 && time > 0)
        {
            timeRecorder = Time.time;
            time--;
            UpdateScore();
        }

        // restart
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // gameover text
        if (gameOver)
        {
            GameOver();
        }

        // 金手指
        if (Input.GetKeyDown(KeyCode.P))
        {
            time = 0;
        }
    }

    // check game over
    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkWait);
            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

    void UpdateScore()
    {
        timeText.text = "Time: " + time;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
    }
}
