﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] roomWaves;
    public int totalWaveIndex =  0;

    private EnemySpawner enemySpawner;

    public List<GameObject> activeEnemies;

    private void Awake()
    {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        //Invoke("SpawnWave", 1.5f);
        StartCoroutine(nameof(SpawnWaveCoroutine));
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

    public void AddActiveEnemy(GameObject enemyInput)
    {
        activeEnemies.Add(enemyInput);
    }

    public void RemoveActiveEnemy(GameObject enemyInput)
    {
        if (!activeEnemies.Contains(enemyInput))
        {
            return;
        }

        activeEnemies.Remove(enemyInput);
    }

    IEnumerator SpawnWaveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < roomWaves[totalWaveIndex].enemies.Length; i++)
        {
            enemySpawner.SpawnEnemy(roomWaves[totalWaveIndex].enemies[i], enemySpawner.SpawnPoints[i]);
            yield return new WaitForSeconds(roomWaves[totalWaveIndex].spawnInterval);
        }

        totalWaveIndex++;
        StopCoroutine(nameof(SpawnWaveCoroutine));
    }

}