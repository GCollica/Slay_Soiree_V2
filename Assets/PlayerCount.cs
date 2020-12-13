using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCount : MonoBehaviour
{
    private UIHandler uIHandler;
    public List<GameObject> playerInputManagers;
    public List<GameObject> activePlayers;
    public List<GameObject> enemies;

    private void Awake()
    {
        uIHandler = FindObjectOfType<UIHandler>();
    }

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
        uIHandler.InitialisePlayers();
    }

    public void RemovePlayerInputManager(GameObject player)
    {
        playerInputManagers.Remove(player);
    }
}
