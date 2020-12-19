using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public List<Sprite> playerBanners;

    private PlayerCount playerCount;
    public Animator animator; 

    public int spawnIndex = 0;

    public bool gameStarted;

    private void Awake()
    {
        playerCount = FindObjectOfType<PlayerCount>();
    }

    public void SetIndex()
    {
        spawnIndex++;
    }

    public void PlayerStarted()
    {
        gameStarted = true;
    }

    public void LoseState()
    {
        if (gameStarted == true && playerCount.activePlayers.Count == 0)
        {
            animator.SetTrigger("Lost");
        }
    }

    public void ShowLostBanner()
    {

    }
}
