using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int enemyCount;
    [SerializeField] int killCount;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject gameOverScreen;
    LevelManager lManager;

    float timer;
    public static GameManager instance;
    int score;
    bool bossKilled = false;
    public enum SceneType
    {
        Regular, Boss
    }
    public SceneType sceneType;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        lManager = FindObjectOfType<LevelManager>();
        EnemyShip.killedByPlayer += EnemyKilledByPlayer;
        EnemyShip.bossDeath += OnBossKilled;
        PlayerController.gameOver += GameOver;
        score = DataManager.instance.GetScore();
    }

    void Update()
    {
        switch (sceneType)
        {
            case SceneType.Regular:
                if (timer >= timeToSpawn)
                {
                    if (enemyCount > 0)
                    {
                        Instantiate(enemies[Random.Range(0, enemies.Length)], lManager.Spawners[Random.Range(0, lManager.Spawners.Count)]);
                        timer = 0;
                    }
                }
                if (enemyCount <= 0)
                {
                    DataManager.instance.SetScore(score);
                    Victory();
                }
                break;
            case SceneType.Boss:
                if(bossKilled) StartCoroutine(OnBossDeath(3));
                break;
        }
        timer += Time.deltaTime;
    }
    void EnemyKilledByPlayer(EnemyShip es)
    {
        enemyCount--;
        score += 250;
    }
    void OnBossKilled(EnemyShip es)
    {
        bossKilled = true;
    }
    void GameOver(PlayerController pc)
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
    void Victory()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
    }
    public int GetScore()
    {
        return score;
    }
    private void OnDisable()
    {
        EnemyShip.killedByPlayer -= EnemyKilledByPlayer;
        EnemyShip.bossDeath -= OnBossKilled;
        PlayerController.gameOver -= GameOver;
        instance = null;
    }
    IEnumerator OnBossDeath(float time)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(time);
        Victory();
    }

}
