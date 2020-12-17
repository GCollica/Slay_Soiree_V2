using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public List<Sprite> playerBanners;

    public int spawnIndex = 0;

    public void SetIndex()
    {
        spawnIndex++;
    }
}
