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
    private PlayerStats player1Stats;
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

    public TMPro.TextMeshProUGUI P1HealthPotValueText;
    public TMPro.TextMeshProUGUI P2HealthPotValueText;
    public TMPro.TextMeshProUGUI P3HealthPotValueText;
    public TMPro.TextMeshProUGUI P4HealthPotValueText;

    public GameObject P1PressStart;
    public GameObject P2PressStart;
    public GameObject P3PressStart;
    public GameObject P4PressStart;


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
                    P1HealthBar.fillAmount = 0f;
                    P1GoldValueText.text = " ";
                    P1HealthPotValueText.text = " ";
                    break;
                }

                if(P1PressStart.activeInHierarchy == true)
                {
                    P1PressStart.SetActive(false);
                }
                

                p1HealthBarValue = player1Stats.playerClass.currentHealth / player1Stats.playerClass.maxHealth;
                P1HealthBar.fillAmount = p1HealthBarValue;

                P1GoldValueText.text = "Gold: " + player1Stats.playerClass.currentGold.ToString();
                P1HealthPotValueText.text = player1Stats.potCount.ToString();

                break;
            case 2:
                if (player2Stats == null)
                {
                    P2HealthBar.fillAmount = 0f;
                    P2GoldValueText.text = " ";
                    P2HealthPotValueText.text = " ";
                    break;
                }

                if (P2PressStart.activeInHierarchy == true)
                {
                    P2PressStart.SetActive(false);
                }

                p2HealthBarValue = player2Stats.playerClass.currentHealth / player2Stats.playerClass.maxHealth;
                P2HealthBar.fillAmount = p2HealthBarValue;

                P2GoldValueText.text = "Gold: " + player2Stats.playerClass.currentGold.ToString();
                P2HealthPotValueText.text = player2Stats.potCount.ToString();

                break;
            case 3:
                if (player3Stats == null)
                {
                    P3HealthBar.fillAmount = 0f;
                    P3GoldValueText.text = " ";
                    P3HealthPotValueText.text = " ";
                    break;
                }

                if (P3PressStart.activeInHierarchy == true)
                {
                    P3PressStart.SetActive(false);
                }

                p3HealthBarValue = player3Stats.playerClass.currentHealth / player3Stats.playerClass.maxHealth;
                P3HealthBar.fillAmount = p3HealthBarValue;

                P3GoldValueText.text = "Gold: " + player3Stats.playerClass.currentGold.ToString();
                P3HealthPotValueText.text = player3Stats.potCount.ToString();
                break;
            case 4:
                if (player4Stats == null)
                {
                    P4HealthBar.fillAmount = 0f;
                    P4GoldValueText.text = " ";
                    P4HealthPotValueText.text = " ";
                    break;
                }

                if (P4PressStart.activeInHierarchy == true)
                {
                    P4PressStart.SetActive(false);
                }

                p4HealthBarValue = player4Stats.playerClass.currentHealth / player4Stats.playerClass.maxHealth;
                P4HealthBar.fillAmount = p4HealthBarValue;

                P4GoldValueText.text = "Gold: " + player4Stats.playerClass.currentGold.ToString();
                P4HealthPotValueText.text = player4Stats.potCount.ToString();
                break;

            default:
                break;
        }
    }

}
