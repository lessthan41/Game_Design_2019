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

    // 遊戲開始間隔時間
    public float startWait;
    public float checkWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    public static bool gameOver;
    public static int score;
    private bool restart;

    EntityManager manager;

    Entity enemyEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards, World.Active);

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
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (gameOver)
        {
            GameOver();
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        Vector3 spawnPosition = new Vector3(spawnValues.x, spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;

        Entity enemy = manager.Instantiate(enemyEntityPrefab);
        manager.SetComponentData(enemy, new Translation { Value = spawnPosition });
        manager.SetComponentData(enemy, new Rotation { Value = Quaternion.Normalize(spawnRotation) });

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

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
