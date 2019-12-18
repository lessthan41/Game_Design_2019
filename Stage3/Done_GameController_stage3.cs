using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;

public class Done_GameController_stage3 : MonoBehaviour
{
    public GameObject hazard;
    public EnemyShooting_stage3 EnemyShooting;

    public float bossWait;
    public float startWait;
    public int WIN_SCORE;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    // public HealthBar healthBar;

    public static bool gameOver;
    public static int score;
    public static bool bossShow;

    public static bool bossMove;
    public static bool moveForward;

    private int recordScore;
    private bool restart;

    EntityManager manager;
    Entity enemyEntityPrefab;

    void Start()
    {
        manager = World.Active.EntityManager;
        enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazard, World.Active);

        gameOver = false;
        score = 0;
        bossShow = false;
        restart = false;

        bossMove = false;
        moveForward = false;

        restartText.text = "";
        gameOverText.text = "";
        recordScore = 0;

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
            EnemyShooting.SetStartShooting (false);
            if (score < WIN_SCORE)
            {
                GameOver();
                restartText.text = "Press 'R' for Restart";
                restart = true;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            bossMove = (bossMove == true) ? false : true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            moveForward = (moveForward == true) ? false : true;
        }

        // 金手指
        if (Input.GetKeyDown(KeyCode.P))
        {
            score = 9999;
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        HealthBar.showHealth = true;
        yield return new WaitForSeconds(bossWait);
        bossShow = true;

        yield return new WaitForSeconds(3.5f);

        // Boss
        Entity boss = manager.Instantiate(enemyEntityPrefab);
        Quaternion spawnRotation = Quaternion.identity;
        manager.SetComponentData(boss, new Translation { Value = new Vector3(0f, 0f, 5.6f) });
        manager.SetComponentData(boss, new Rotation { Value = Quaternion.Normalize(spawnRotation) });

        yield return new WaitForSeconds(2);

        EnemyShooting.SetStartShooting (true);

        EnemyShooting.SetFireOrNot ("round", true);
        EnemyShooting.SetFireRate (3, 0.1f);
        EnemyShooting.SetSpawnAmount (3, 6);

        yield return new WaitForSeconds(10);

        EnemyShooting.SetFireOrNot ("round", false);


    }

    public float GetStartWait ()
    {
        return startWait;
    }

    public int GetScore ()
    {
        return score;
    }

    public int GetWinScore ()
    {
        return WIN_SCORE;
    }

    public bool GetGameStatus ()
    {
        return gameOver;
    }

    public static void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        if (score > 9999)
            score = 9999;
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
