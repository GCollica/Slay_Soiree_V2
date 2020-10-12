using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    //public PlayerCount playerCountComponent;
    public PlayerStats player1Stats;
    public Image p1Image;
    private float p1ImageValue;
    private PlayerStats player2Stats;
    private PlayerStats player3Stats;
    private PlayerStats player4Stats;

    // Start is called before the first frame update
    void Awake()
    {
        //playerCountComponent = FindObjectOfType<PlayerCount>();
    }

    private void Update()
    {
        //InitialisePlayers();

        p1ImageValue = player1Stats.playerClass.currentHealth / 100f;
        p1Image.fillAmount = p1ImageValue;
    }

    /*public void InitialisePlayers()
    {
        if(player1Stats == null)
        {
            player1Stats = playerCountComponent.players[1].GetComponent<PlayerStats>();
        }

    }*/




}
