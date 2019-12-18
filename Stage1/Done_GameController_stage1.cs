using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;

public class Done_GameController_stage1 : MonoBehaviour
{
    public GameObject hazards;
    public Vector3 spawnValues;

    // 遊戲時間、遊戲開始間隔時間
    public int GameTime;
    public float GameStartWait;

    public float checkWait;
    public static float startWait;
    public static int time;

    public Text timeText;
    public Text restartText;
    public Text gameOverText;

    private float timeRecorder;
    public static bool gameOver;
    private bool restart;

    EntityManager manager;
    Entity enemyEntityPrefab;

    void Start()
    {
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
        StartCoroutine(SpawnWaves());
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
