using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

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
    int liveEnemies;
    public static GameManager instance;
    int score;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        lManager = FindObjectOfType<LevelManager>();
        EnemyShip.onDestroy += DestroyEnemy;
        EnemyShip.killedByPlayer += EnemyKilledByPlayer;
        PlayerController.gameOver += GameOver;
        score = DataManager.instance.GetScore();
    }

    void Update()
    {
        if (timer >= timeToSpawn)
        {
            if (enemyCount > 0)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], lManager.Spawners[Random.Range(0, lManager.Spawners.Count)]);
                enemyCount--;
                liveEnemies++;
                timer = 0;
            }
        }
        if(enemyCount<=0 && liveEnemies <= 0)
        {
            DataManager.instance.SetScore(score);
            Victory();
        }
        timer += Time.deltaTime;
    }
    void DestroyEnemy(EnemyShip es)
    {
        liveEnemies--;
    }
    void EnemyKilledByPlayer(EnemyShip es)
    {
        liveEnemies--;
        score += 250;
    }
    void GameOver(PlayerController pc)
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
    void Victory()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public int GetScore()
    {
        return score;
    }
    private void OnDisable()
    {
        EnemyShip.onDestroy -= DestroyEnemy;
        EnemyShip.killedByPlayer -= EnemyKilledByPlayer;
        PlayerController.gameOver -= GameOver;
        instance = null;
    }

}
