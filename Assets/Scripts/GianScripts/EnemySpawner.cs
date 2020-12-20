using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    private GameObject EnemyParent;

    public RoomSpawnPoints roomSpawnPoints;

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseEnemyParent();
        InitialiseSpawnPoints();
    }

    public void InitialiseEnemyParent()
    {
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent");
    }
    public void InitialiseSpawnPoints()
    {
        roomSpawnPoints = FindObjectOfType<RoomSpawnPoints>();
        SpawnPoints.Clear();

        foreach (Transform spawnPoints in roomSpawnPoints.enemySpawnPoints)
        {
            SpawnPoints.Add(spawnPoints);
        }
    }

    public void SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {
        GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        spawnedEnemy.transform.SetParent(EnemyParent.transform);
    }

    public void ReassignRoomReferences()
    {
        InitialiseEnemyParent();
        InitialiseSpawnPoints();
    }
}
