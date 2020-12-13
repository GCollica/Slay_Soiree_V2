using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public PlayerCount playerCountComponent;

    /// <summary>
    /// Player Stat fields
    /// </summary>
    public PlayerStats player1Stats;
    private PlayerStats player2Stats;
    private PlayerStats player3Stats;
    private PlayerStats player4Stats;

    /// <summary>
    /// Healthbar fields
    /// </summary>
    public Image P1HealthBar;
    private float p1HealthBarValue;
    public Image P2HealthBar;
    private float p2HealthBarValue;
    public Image P3HealthBar;
    private float p3HealthBarValue;
    public Image P4HealthBar;
    private float p4HealthBarValue;

    /// <summary>
    /// Gold value fields
    /// </summary>
    public TMPro.TextMeshProUGUI P1GoldValueText;
    public TMPro.TextMeshProUGUI P2GoldValueText;
    public TMPro.TextMeshProUGUI P3GoldValueText;
    public TMPro.TextMeshProUGUI P4GoldValueText;


    // Start is called before the first frame update
    void Awake()
    {
        playerCountComponent = FindObjectOfType<PlayerCount>();
    }

    private void Update()
    {
        UpdateUI(1);
        UpdateUI(2);
        UpdateUI(3);
        UpdateUI(4);
    }

    public void InitialisePlayers()
    {
        for (int i = 0; i < playerCountComponent.playerInputManagers.Count; i++)
        {
            if(i == 0)
            {
                if (player1Stats == null)
                {
                    player1Stats = playerCountComponent.playerInputManagers[0].GetComponentInParent<PlayerStats>();
                }
            }

            if(i == 1)
            {
                if (player2Stats == null)
                {
                    player2Stats = playerCountComponent.playerInputManagers[1].GetComponentInParent<PlayerStats>();
                }
            }

            if(i == 2)
            {
                if (player3Stats == null)
                {
                    player3Stats = playerCountComponent.playerInputManagers[2].GetComponentInParent<PlayerStats>();
                }
            }

            if(i == 3)
            {
                if (player4Stats == null)
                {
                    player4Stats = playerCountComponent.playerInputManagers[3].GetComponentInParent<PlayerStats>();
                }
            }
        }

    }

    private void UpdateUI(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                if (player1Stats == null)
                {
                    break;
                }

                p1HealthBarValue = player1Stats.playerClass.currentHealth / 100f;
                P1HealthBar.fillAmount = p1HealthBarValue;

                P1GoldValueText.text = "Gold: " + player1Stats.playerClass.currentGold.ToString();

                break;
            case 2:
                if (player2Stats == null)
                {
                    break;
                }

                p2HealthBarValue = player2Stats.playerClass.currentHealth / 100f;
                P2HealthBar.fillAmount = p2HealthBarValue;

                P2GoldValueText.text = "Gold: " + player2Stats.playerClass.currentGold.ToString();

                break;
            case 3:
                if (player3Stats == null)
                {
                    break;
                }

                p3HealthBarValue = player3Stats.playerClass.currentHealth / 100f;
                P3HealthBar.fillAmount = p3HealthBarValue;

                P3GoldValueText.text = "Gold: " + player3Stats.playerClass.currentGold.ToString();
                break;
            case 4:
                if (player4Stats == null)
                {
                    break;
                }

                p4HealthBarValue = player4Stats.playerClass.currentHealth / 100f;
                P4HealthBar.fillAmount = p4HealthBarValue;

                P4GoldValueText.text = "Gold: " + player4Stats.playerClass.currentGold.ToString();
                break;

            default:
                break;
        }
    }

}
