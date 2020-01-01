using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Transforms;

// control stage 2
public class Done_GameController_stage2 : MonoBehaviour
{
    public GameObject[] hazards; // hazard container
    public Vector3 spawnValues; // enemy instantiate point
    public int hazardCount; // hazard numbers

    // time setting
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float bossWait;
    public int WIN_SCORE; // for fade out trigger

    // UI Text
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    // variable for communicating with system
    public static bool gameOver;
    public static int score;
    public static bool bossShow;
    public static bool ranpage;

    // status variable
    private int recordScore;
    private bool restart;
    [SerializeField] private HealthBar healthBar; // boss health bar

    EntityManager manager; // entity instantiate manager
    Entity enemyEntityPrefab0; // put enemy prefab
    Entity enemyEntityPrefab1; // put enemy prefab

    void Start()
    {
        // initialize enemy prefab for instantiate
        manager = World.Active.EntityManager;
        enemyEntityPrefab0 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[0], World.Active);
        enemyEntityPrefab1 = GameObjectConversionUtility.ConvertGameObjectHierarchy(hazards[1], World.Active);

        gameOver = false;
        WIN_SCORE = 9999;
        score = 0;
        bossShow = false;
        ranpage = false;
        restart = false;

        restartText.text = "";
        gameOverText.text = "";
        recordScore = 0;

        UpdateScore();
        StartCoroutine(SpawnWaves()); // start hazard wave
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
            EnemyShooting_stage2.startShooting = false;
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
            bossShow = false;
            gameOver = true;
        }

        // 金手指
        if (Input.GetKeyDown(KeyCode.P))
        {
            score = 9999;
        }
    }

    // start instantiate enemies (main procedure)
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        EnemyShooting_stage2.startShooting = true;

        Quaternion spawnRotation = Quaternion.identity;

        for (int round = 0; round < 3; round++)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

                Entity enemy = manager.Instantiate(enemyEntityPrefab0);
                manager.SetComponentData(enemy, new Translation { Value = spawnPosition });
                manager.SetComponentData(enemy, new Rotation { Value = Quaternion.Normalize(spawnRotation) });

                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);
        }

        EnemyShooting_stage2.startShooting = false;
        HealthBar.showHealth = true;

        yield return new WaitForSeconds(bossWait);

        // Boss
        Entity boss = manager.Instantiate(enemyEntityPrefab1);
        manager.SetComponentData(boss, new Translation { Value = new Vector3(0f, 0f, 18f) });
        manager.SetComponentData(boss, new Rotation { Value = Quaternion.Normalize(spawnRotation) });

        yield return new WaitForSeconds(3);

        bossShow = true;
        EnemyShooting_stage2.startShooting = true;


        while (true)
        {
            EnemyShooting_stage2.isLaser = true;

            // laser
            yield return new WaitForSeconds(5);
            EnemyShooting_stage2.isLaser = false;

            yield return new WaitForSeconds(3);
            EnemyShooting_stage2.isUnit = false;
            EnemyShooting_stage2.isSpawn = true;

            // spawn
            yield return new WaitForSeconds(3);
            EnemyShooting_stage2.isSpawn = false;
            EnemyShooting_stage2.isUnit = true;

            yield return new WaitForSeconds(3);
            EnemyShooting_stage2.isUnit = false;
            EnemyShooting_stage2.isRound = true;

            // round
            yield return new WaitForSeconds(10);
            EnemyShooting_stage2.isRound = false;
            EnemyShooting_stage2.isUnit = true;

            yield return new WaitForSeconds(2);
        }

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

    public float GetStartWait ()
    {
        return startWait;
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
    }

}
