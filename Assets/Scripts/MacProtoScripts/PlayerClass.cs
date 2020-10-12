using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerClass
{
    public float currentHealth;
    public float currentResistance;
    public float currentLightDamage;
    public float currentHeavyDamage;
    public float currentAttackRange;
    public float currentMovementSpeed;
    public int currentGold;

    public WeaponsSO currentWeapon;
    public ArmourSO currentArmour;

    public PlayerClass(float healthInput, float resistanceInput, float lightDamageInput, float heavyDamageInput, float attackRangeInput, float movementSpeedInput, int goldInput, WeaponsSO weaponInput, ArmourSO armourInput)
    {
        currentHealth = healthInput;
        currentLightDamage = lightDamageInput;
        currentHeavyDamage = heavyDamageInput;
        currentAttackRange = attackRangeInput;
        currentMovementSpeed = movementSpeedInput;
        currentGold = goldInput;
        currentWeapon = weaponInput;
        currentArmour = armourInput;
    }

    public void TakeCalculatedDamage(float incomingDamage)
    {
        float resCalculated = incomingDamage * (1 - (currentResistance / 100f));
        Debug.Log(resCalculated);
        currentHealth -= resCalculated;
    }

    public void GainGold(int goldInput)
    {
        currentGold += goldInput;
    }
    
    public void UpdateWeaponStats()
    {
        currentLightDamage = currentWeapon.lightDamage;
        currentHeavyDamage = currentWeapon.heavyDamage;
    }

    public void UpdateArmourStats()
    {
        currentResistance = currentArmour.resistance;
        currentMovementSpeed = currentArmour.movementSpeed;
    }

    public void PurchaseNewWeapon(WeaponsSO newWeapon)
    {
        if(currentGold < newWeapon.cost)
        {
            return;
        }
        else
        {
            currentGold -= newWeapon.cost;
            currentWeapon = newWeapon;
            UpdateWeaponStats();
        }      
    }

    public void PurchaseNewArmour(ArmourSO newArmour)
    {
        if(currentGold < newArmour.cost)
        {
            return;
        }
        else
        {
            currentGold -= newArmour.cost;
            currentArmour = newArmour;
            UpdateArmourStats();
        }        
    }
}
