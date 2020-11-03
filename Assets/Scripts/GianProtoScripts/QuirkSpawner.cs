using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkSpawner : MonoBehaviour
{
    private GameObject quirkParent;
    public List<Transform> SpawnPoints;

    private RoomSpawnPoints roomSpawnPoints;

    private void Awake()
    {
        InitialiseQuirkParent();
        InitialiseSpawnPoints();
    }
    public void InitialiseQuirkParent()
    {
        quirkParent = GameObject.FindGameObjectWithTag("QuirkParent");
    }

    public void InitialiseSpawnPoints()
    {
        SpawnPoints.Clear();

        roomSpawnPoints = FindObjectOfType<RoomSpawnPoints>();

        foreach (Transform spawnPoint in roomSpawnPoints.quirkSpawnPoints)
        {
            SpawnPoints.Add(spawnPoint);
        }
    }

    public void SpawnQuirkObject(GameObject quirkObj)
    {
        int chosenSpawnIndex = Mathf.RoundToInt(Random.Range(0, SpawnPoints.Count));
        GameObject spawnedQuirkObj = Instantiate(quirkObj,  SpawnPoints[chosenSpawnIndex].position, Quaternion.identity);
        spawnedQuirkObj.transform.SetParent(quirkParent.transform);
    }

    public void ReassignRoomReferences()
    {
        InitialiseQuirkParent();
        InitialiseSpawnPoints();
    }
}
