using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int commonEnemyCount;
    [SerializeField] int erraticEnemyCount;
    [SerializeField] int killCount;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] List<GameObject> liveEnemies;
    LevelManager lManager;

    float timer;
    public static GameManager instance;
    int score;
    int index = 0;
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
        EnemyShip.onDestroy += OnEnemyDestroy;
        EnemyShip.killedByPlayer += EnemyKilledByPlayer;
        EnemyShip.bossDeath += OnBossKilled;
        PlayerController.gameOver += GameOver;
        score = DataManager.instance.GetScore();
        if(sceneType == SceneType.Regular)
        {
            for (int i = 0; i < commonEnemyCount; i++)
            {
                GameObject go = Instantiate(enemies[0], lManager.Spawners[Random.Range(0, lManager.Spawners.Count)]);
                liveEnemies.Add(go);
                liveEnemies[i].SetActive(false);
            }
            for (int i = 0; i < erraticEnemyCount; i++)
            {
                GameObject go = Instantiate(enemies[1], lManager.Spawners[Random.Range(0, lManager.Spawners.Count)]);
                liveEnemies.Add(go);
                liveEnemies[i].SetActive(false);
            }
        }

    }

    void Update()
    {
        switch (sceneType)
        {
            case SceneType.Regular:
                for (int i = 0; i < liveEnemies.Count; i++)
                {
                    if (liveEnemies[i] == null)
                    {
                        liveEnemies.RemoveAt(i);
                    }
                }


                if(index>liveEnemies.Count-1) index = 0;

                if (timer >= timeToSpawn)
                {
                    if (liveEnemies.Count>0)
                    {
                        for(int i = 0; i<liveEnemies.Count; i++)
                        {
                            if (!liveEnemies[i].activeSelf)
                            {
                                liveEnemies[i].transform.position = lManager.Spawners[Random.Range(0, lManager.Spawners.Count)].position;
                                liveEnemies[i].SetActive(true);
                                break;
                            }
                        }
                        index++;
                        timer = 0;
                    }
                }
                if (liveEnemies.Count <= 0)
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
        score += 250;

    }
    void OnEnemyDestroy(EnemyShip es)
    {
        for (int i = 0; i < liveEnemies.Count; i++)
        {
            if (!liveEnemies[i].activeSelf)
            {
                liveEnemies[i].transform.position = lManager.Spawners[Random.Range(0, lManager.Spawners.Count)].position;
            }
        }
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
        EnemyShip.onDestroy -= OnEnemyDestroy;
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
