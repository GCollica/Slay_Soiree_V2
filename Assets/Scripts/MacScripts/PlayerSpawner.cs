using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public List<Sprite> playerBanners;

    private PlayerCount playerCount;   

    public int spawnIndex = 0;

    public bool gameStarted;

    public Animator blackImageAnimator;

    private void Awake()
    {
        playerCount = FindObjectOfType<PlayerCount>();
    }

    private void Update()
    {
        LoseState();
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
        if (gameStarted == true && playerCount.playerInputManagers.Count == 0)
        {
            Debug.Log("LostSequence");
            blackImageAnimator.SetTrigger("Lost");
        }
    }
}
