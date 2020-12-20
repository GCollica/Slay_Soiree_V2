using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveSO[] roomWaves;
    private int totalWaveIndex =  0;
    private int spawnPointIndex = 0;

    private EnemySpawner enemySpawner;
    private RoomProgress roomProgress;

    public List<GameObject> activeEnemies;

    private bool spawningWave = false;


    private void Awake()
    {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        roomProgress = this.gameObject.GetComponent<RoomProgress>();
    }

    /*public void SpawnWave()
    {
        int spawnPointIndex = 0;

        foreach (GameObject enemy in roomWaves[totalWaveIndex].enemies)
        {
            enemySpawner.SpawnEnemy(enemy, enemySpawner.SpawnPoints[spawnPointIndex]);
            spawnPointIndex++;
        }

        totalWaveIndex++;
    }*/

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
        WaveCountCheck();
    }

    IEnumerator SpawnWaveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        //Debug.Log(totalWaveIndex);
        //Debug.Log(roomWaves[totalWaveIndex].waveName);

        for (int i = 0; i < roomWaves[totalWaveIndex].enemies.Length; i++)
        {
            enemySpawner.SpawnEnemy(roomWaves[totalWaveIndex].enemies[i], enemySpawner.SpawnPoints[spawnPointIndex]);

            if(spawnPointIndex == enemySpawner.SpawnPoints.Count -1)
            {
                spawnPointIndex = 0;
            }
            else
            {
                spawnPointIndex++;
            }
            yield return new WaitForSeconds(roomWaves[totalWaveIndex].spawnInterval);
        }

        totalWaveIndex++;
        spawnPointIndex = 0;
        spawningWave = false;
        StopCoroutine(nameof(SpawnWaveCoroutine));
    }

    public void WaveCountCheck()
    {
        if(activeEnemies.Count > 0)
        {
            return;
        }

        if(totalWaveIndex < roomWaves.Length && spawningWave == false)
        {
            spawningWave = true;
            StartCoroutine(nameof(SpawnWaveCoroutine));
        }
        else if(totalWaveIndex == roomWaves.Length && spawningWave == false)
        {
            roomProgress.ChangeRoomState(RoomProgress.RoomState.Completed);
        }
    }

    public void BeginRoomWaves()
    {
        StartCoroutine(nameof(SpawnWaveCoroutine));
    }

}
