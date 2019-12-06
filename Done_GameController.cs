using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;

    // 產生怪物間隔時間
    public float spawnWait;
    // 遊戲開始間隔時間
    public float startWait;
    // 每波怪物間隔時間
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    public static bool gameOver;
    public static int WIN_SCORE = 100;
    public static int score;

    public int recordScore;
    private bool restart;

    EntityManager manager;

    Entity enemyEntityPrefab0;
    Entity enemyEntityPrefab1;
    Entity enemyEntityPrefab2;
    Entity enemyEntityPrefab3;

    void Start()
    {
        manager = World.Active.EntityManager;
        enemyEntityPrefab0 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[0], World.Active);
        enemyEntityPrefab1 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[1], World.Active);
        enemyEntityPrefab2 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[2], World.Active);
        enemyEntityPrefab3 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[3], World.Active);

        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        // restart
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // show gameOver or not
        if (gameOver)
        {
            if (score < WIN_SCORE)
            {
                GameOver();
            }
        }

        // add score
        if (recordScore != score)
        {
            recordScore = score;
            UpdateScore();
        }

        // win
        if (score >= WIN_SCORE)
        {
            gameOver = true;
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                Entity enemy = manager.Instantiate(enemyEntityPrefab3);
                manager.SetComponentData(enemy, new Translation { Value = spawnPosition });
                manager.SetComponentData(enemy, new Rotation { Value = Quaternion.Normalize(spawnRotation) });

                yield return new WaitForSeconds(spawnWait);
            }
            
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

    public static void AddScore(int newScoreValue)
    {
        score += newScoreValue;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        // gameOver = true;
    }
}
