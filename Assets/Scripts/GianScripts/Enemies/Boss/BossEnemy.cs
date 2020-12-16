using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public BossEnemy_Class BossEnemyClass;

    public float startingDamage = 30f;
    public float startingResistance = 50f;
    public float startingHealth = 100f;
    public float startingMovespeed = 50f;

    private void Awake()
    {
        BossEnemyClass = this.gameObject.GetComponent<BossEnemy_Class>();
    }

    private void InitialiseClassInstance()
    {
        BossEnemyClass = new BossEnemy_Class(startingDamage, startingResistance, startingHealth, startingMovespeed);
    }

    public void TakeDamage(GameObject player, string attackType)
    {
        if (attackType == "Heavy")
        {
            BossEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentHeavyDamage);
        }
        else if (attackType == "Light")
        {
            BossEnemyClass.TakeCalculatedDamage(player.GetComponent<PlayerStats>().playerClass.currentLightDamage);
        }

        // If enemy health drops below zero
        if (BossEnemyClass.currentHealth <= 0)
        {
            // Death animation here.
            BeginEnemyDeath();
        }
    }

    private void BeginEnemyDeath()
    {
        //WinState
    }
}
