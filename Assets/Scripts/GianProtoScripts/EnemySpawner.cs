using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public GameObject EnemyParent;

    public GameObject skeletonBasicEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseEnemyParent();
        InitialiseSpawnPoints();

        SpawnEnemy(skeletonBasicEnemy, SpawnPoints[0]);
        SpawnEnemy(skeletonBasicEnemy, SpawnPoints[1]);
        SpawnEnemy(skeletonBasicEnemy, SpawnPoints[2]);
        SpawnEnemy(skeletonBasicEnemy, SpawnPoints[3]);

    }

    public void InitialiseEnemyParent()
    {
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent");
    }
    public void InitialiseSpawnPoints()
    {
        foreach (var spawnPoints in EnemyParent.transform.GetChild(0).transform.GetComponentsInChildren<Transform>())
        {
            SpawnPoints.Add(spawnPoints);
        }

        SpawnPoints.RemoveAt(0);
    }

    public void SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {
        GameObject spawnedEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        spawnedEnemy.transform.SetParent(EnemyParent.transform);
    }
}
