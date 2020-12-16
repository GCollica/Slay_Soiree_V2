﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossEnemy_Class
{
    /*Base class for all boss enemies in the game, holds values for Damage, Resistance, Current Health, Movement Speed. Also includes commonly used functions shared by all common enemies. Specific functions used by each enemy type individually will be housed in their BasicEnemyX scrpit instead.*/

    public float currentDamage;
    public float currentResistance;
    public float currentHealth;
    public float currentMovementSpeed;

    public BossEnemy_Class(float damageInput, float resistanceInput, float healthInput, float movementSpeedInput)
    {
        currentDamage = damageInput;
        currentResistance = resistanceInput;
        currentHealth = healthInput;
        currentMovementSpeed = movementSpeedInput;

    }

    //Calculates the damage taken given an input value against the resistance of the enemy. Then updates current health to accomodate. 
    public void TakeCalculatedDamage(float incomingDamage)
    {
        float resCalculated = incomingDamage * (1 - (currentResistance / 100f));
        //Debug.Log(resCalculated);
        currentHealth -= resCalculated;
    }
}
