using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float startingHealth;
    public int startingGold;

    public WeaponsSO startingWeapon;
    public ArmourSO startingArmour;

    // For Death Totem Quirk damage increase
    public bool damageTotem;
    public float damageMultiplyer = 2f;

    public PlayerClass playerClass;
    private PlayerCount playerCount;
    private PlayerCombat playerCombat;
    public ComboSystem comboSystem;
    private Animator animator;
    private SoundManager soundManager;

    public int startingPotCount;
    [HideInInspector]
    public int potCount;

    public float potency = 25f;

    void Awake()
    {
        playerCount = FindObjectOfType<PlayerCount>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        animator = GetComponentInChildren<Animator>();
        potCount = startingPotCount;
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        InitialiseClassInstance();

        playerClass.UpdateArmourStats();
        playerClass.UpdateWeaponStats();
    }

    private void InitialiseClassInstance()
    {
        playerClass = new PlayerClass(startingHealth, startingArmour.resistance, startingWeapon.lightDamage, startingWeapon.heavyDamage,  startingWeapon.attackRange,startingArmour.movementSpeed, startingGold, startingWeapon, startingArmour);
    }

    public void TakeDamage(float incomingDamage)
    {
        if (damageTotem)
        {
            playerClass.TakeCalculatedDamage(incomingDamage * damageMultiplyer);
        }
        else
        {
            playerClass.TakeCalculatedDamage(incomingDamage);
        }
           

        if(playerClass.currentHealth <= 0)
        {
            Debug.Log("Player " + (playerCombat.playerIndex + 1) + " has died!");

            comboSystem.stateRanged = ComboSystem.RangedState.RestrictMovement;
            // Kill player
            animator = GetComponentInChildren<Animator>();
            animator.SetTrigger("Dead");
        }
    }

    public void KillPlayer()
    {
        playerCount.RemovePlayerInputManager(playerCount.playerInputManagers[playerCombat.playerIndex]);
        playerCombat.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void AddHealth()
    {
        if (potCount >= 1)
        {
            soundManager.Play("Health Pot");
            playerClass.AddHealth(potency);
            potCount--;
        }
        else
        {
            Debug.Log("No more pots");
        }        
    }

    public void AddPot(int amount)
    {
        potCount += amount;
    }

    //Call this when interact is pressed to purchase weapon.
    public void UpdateCurrentItem(Collider2D pedistoolCollider)
    {
        if(pedistoolCollider.gameObject.GetComponent<ItemPedistool>().currentItemType != ItemPedistool.ItemType.Empty)
        {
            if(pedistoolCollider.gameObject.GetComponent<ItemPedistool>().currentItemType == ItemPedistool.ItemType.Weapon)
            {
                playerClass.PurchaseNewWeapon(pedistoolCollider.gameObject.GetComponent<ItemPedistool>().currentWeaponItem);
            }
            else if(pedistoolCollider.gameObject.GetComponent<ItemPedistool>().currentItemType == ItemPedistool.ItemType.Armour)
            {
                playerClass.PurchaseNewArmour(pedistoolCollider.gameObject.GetComponent<ItemPedistool>().currentArmourItem);
            }
        }
        else
        {
            return;
        }
    }
}
