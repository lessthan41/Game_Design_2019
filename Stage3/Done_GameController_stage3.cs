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
    public int bossMode;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    // static for system
    public static bool gameOver;
    public static int score;
    public static bool bossShow;

    // boss move or not
    public static bool bossMove;
    public static bool moveForward;

    public static bool bossDirectionChange;
    public static bool bossSpeedChange;
    public static float bossSpeedRate;

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

        bossDirectionChange = false;
        bossSpeedChange = false;
        bossSpeedRate = 1.0f;

        restartText.text = "";
        gameOverText.text = "";
        recordScore = 0;

        UpdateScore();
        StartCoroutine(GameStart());
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

    IEnumerator GameStart ()
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

        // game start
        EnemyShooting.SetStartShooting (true);

        yield return StartCoroutine( One() );
    }

    IEnumerator One ()
    {
        // round
        EnemyShooting.SetFireRate ("round", 0.1f);
        EnemyShooting.SetSpawnAmount ("round", 6);
        EnemyShooting.SetFireOrNot ("round", true);
        yield return new WaitForSeconds(10);
        EnemyShooting.SetFireOrNot ("round", false);

        // move backward
        BossSpeedChange (2f);
        MoveForwardSwitch (true);
        yield return new WaitForSeconds(1);
        MoveForwardSwitch (false);

        yield return StartCoroutine( BaseRoutine() );
    }

    IEnumerator BaseRoutine ()
    {
        while (bossMode == 1)
        {
            BossMoveSwitch (true);
            EnemyShooting.SetFireOrNot("unit", true);
            yield return new WaitForSeconds(5);
            EnemyShooting.SetFireOrNot("unit", false);
            BossMoveSwitch (false);

            // round
            EnemyShooting.SetFireRate ("spawn", 0.1f);
            EnemyShooting.SetSpawnAmount ("spawn", 9);
            EnemyShooting.SetFireOrNot ("spawn", true);
            yield return new WaitForSeconds(3);
            EnemyShooting.SetFireOrNot ("spawn", false);
        }

        yield return StartCoroutine( Two() );
    }

    IEnumerator Two ()
    {
        // move forward
        BossDirectionChange ();
        yield return new WaitForSeconds(2);
        MoveForwardSwitch (true);
        yield return new WaitForSeconds(0.5f);
        MoveForwardSwitch (false);

        // round
        EnemyShooting.SetFireRate ("duo", 1f);
        EnemyShooting.SetSpawnAmount ("duo", 8);
        EnemyShooting.SetFireOrNot ("duo", true);
        yield return new WaitForSeconds(10);
        EnemyShooting.SetFireOrNot ("duo", false);

        // move backward
        BossDirectionChange ();
        yield return new WaitForSeconds(2);
        MoveForwardSwitch (true);
        yield return new WaitForSeconds(0.5f);
        MoveForwardSwitch (false);

        yield return StartCoroutine( MediumRoutine() );
    }

    IEnumerator MediumRoutine ()
    {
        while (bossMode == 2)
        {
            BossMoveSwitch (true);
            EnemyShooting.SetFireRate ("spawn", 0.75f);
            EnemyShooting.SetFireOrNot("spawn", true);
            yield return new WaitForSeconds(3);
            EnemyShooting.SetFireOrNot("spawn", false);
            BossMoveSwitch (false);

            // round
            EnemyShooting.SetFireRate ("duo", 1f);
            EnemyShooting.SetSpawnAmount ("duo", 8);
            EnemyShooting.SetFireOrNot ("duo", true);
            yield return new WaitForSeconds(5);
            EnemyShooting.SetFireOrNot ("duo", false);
        }

        yield return StartCoroutine( Three() );
    }

    IEnumerator Three ()
    {
        // move forward
        BossDirectionChange ();
        yield return new WaitForSeconds(2);
        MoveForwardSwitch (true);
        yield return new WaitForSeconds(1);
        MoveForwardSwitch (false);

        // round
        EnemyShooting.SetFireRate ("round", 0.1f);
        EnemyShooting.SetSpawnAmount ("round", 6);
        EnemyShooting.SetFireOrNot ("round", true);
        yield return new WaitForSeconds(1.5f);
        EnemyShooting.SetRoundDirection ();
        yield return new WaitForSeconds(1.5f);
        EnemyShooting.SetRoundDirection ();
        yield return new WaitForSeconds(1.5f);
        EnemyShooting.SetRoundDirection ();
        yield return new WaitForSeconds(1.5f);
        EnemyShooting.SetFireOrNot ("round", false);

        yield return new WaitForSeconds(5);

        // tur
        EnemyShooting.SetFireRate ("tur", 1f);
        EnemyShooting.SetSpawnAmount ("tur", 6);
        EnemyShooting.SetFireOrNot ("tur", true);
        yield return new WaitForSeconds(5);
        EnemyShooting.SetFireOrNot ("tur", false);

        // move backward
        BossDirectionChange ();
        yield return new WaitForSeconds(2);
        MoveForwardSwitch (true);
        yield return new WaitForSeconds(0.5f);
        MoveForwardSwitch (false);

        yield return StartCoroutine( HardRoutine() );
    }

    IEnumerator HardRoutine ()
    {
        while (bossMode == 3)
        {
            BossMoveSwitch (true);
            EnemyShooting.SetFireRate ("spawn", 0.6f);
            EnemyShooting.SetFireOrNot("spawn", true);
            yield return new WaitForSeconds(1.5f);
            EnemyShooting.SetFireOrNot("spawn", false);
            BossMoveSwitch (false);

            // round
            EnemyShooting.SetFireRate ("tur", 1f);
            EnemyShooting.SetSpawnAmount ("tur", 6);
            EnemyShooting.SetFireOrNot ("tur", true);
            yield return new WaitForSeconds(5);
            EnemyShooting.SetFireOrNot ("tur", false);
        }
    }

    private void MoveForwardSwitch (bool toSet)
    {
        moveForward = toSet;
    }

    private void BossMoveSwitch (bool toSet)
    {
        bossMove = toSet;
    }

    private void BossDirectionChange ()
    {
        bossDirectionChange = true;
    }

    private void BossSpeedChange (float rate)
    {
        bossSpeedChange = true;
        bossSpeedRate = rate;
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

    public void SetBossMode (int mode)
    {
        bossMode = mode;
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
