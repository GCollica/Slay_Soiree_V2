using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTotem : MonoBehaviour
{
    public float damageMultiplyer;
    private BasicEnemy1 enemy;

    private PlayerStats playerStats;

    public float health = 50f; 

    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        // Toggle damage increase true
        playerStats.damageTotem = true;
    }

    public void TotemTakeDamage(float damage)
    {
        Debug.Log("Totem took damage!"); 
        health = health -= damage;

        if (health <= 0)
        {
            // Toggle damage increase false
            playerStats.damageTotem = false;

            Destroy(gameObject);
        }
    }
}
