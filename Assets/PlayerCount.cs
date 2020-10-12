using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCount : MonoBehaviour
{
    public List<GameObject> players;
    public List<GameObject> enemies;

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }

    public void RemovePlayer(GameObject player)
    {
        players.Remove(player);
    }
}
