using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCount : MonoBehaviour
{
    public List<GameObject> playerInputManagers;
    public List<GameObject> activePlayers;
    public List<GameObject> enemies;

    public void UpdateActivePlayers()
    {
        foreach (PlayerStats playerStats in FindObjectsOfType<PlayerStats>())
        {
            activePlayers.Add(playerStats.gameObject);
        }
    }

    public void AddPlayerInputManager(GameObject player)
    {
        playerInputManagers.Add(player);
    }

    public void RemovePlayerInputManager(GameObject player)
    {
        playerInputManagers.Remove(player);
    }
}
