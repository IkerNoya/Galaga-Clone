using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int enemyCount;
    [SerializeField] int killCount;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject[] enemies;
    LevelManager lManager;
    float timer;
    int liveEnemies;

    void Start()
    {
        lManager = FindObjectOfType<LevelManager>();
        EnemyShip.onDestroy += DestroyEnemy;
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
        timer += Time.deltaTime;
    }
    void DestroyEnemy(EnemyShip es)
    {
        liveEnemies--;
    }
    private void OnDisable()
    {
        EnemyShip.onDestroy -= DestroyEnemy;
    }

}
