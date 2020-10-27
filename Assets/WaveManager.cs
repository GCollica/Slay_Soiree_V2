using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] roomWaves;
    public int totalWaveIndex =  0;

    private EnemySpawner enemySpawner;

    private void Awake()
    {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        Invoke("SpawnWave", 1.5f);
    }

    public void SpawnWave()
    {
        int spawnPointIndex = 0;

        foreach (GameObject enemy in roomWaves[totalWaveIndex].enemies)
        {
            enemySpawner.SpawnEnemy(enemy, enemySpawner.SpawnPoints[spawnPointIndex]);
            spawnPointIndex++;
        }

        totalWaveIndex++;
    }

}
