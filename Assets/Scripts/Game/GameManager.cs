using System.Collections;
using System.Collections.Generic;
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
    }

    void Update()
    {
        if (timer >= timeToSpawn)
        {
            if (enemyCount > 0)
            {
                Instantiate(enemies[Random.Range(0, 1)], lManager.Spawners[Random.Range(0, 2)]);
                enemyCount--;
                liveEnemies++;
                timer = 0;
            }
        }
        timer += Time.deltaTime;
    }
}
