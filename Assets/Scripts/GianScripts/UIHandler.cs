using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    //public PlayerCount playerCountComponent;
    public PlayerStats player1Stats;
    public Image P1Image;
    private float p1ImageValue;
    public TMPro.TextMeshProUGUI P1GoldValueText;
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

        //p1ImageValue = player1Stats.playerClass.currentHealth / 100f;
        //P1Image.fillAmount = p1ImageValue;

        //P1GoldValueText.text = "Gold: " + player1Stats.playerClass.currentGold.ToString();
    }

    /*public void InitialisePlayers()
    {
        if(player1Stats == null)
        {
            player1Stats = playerCountComponent.players[1].GetComponent<PlayerStats>();
        }

    }*/




}
