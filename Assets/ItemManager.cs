using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public WeaponsSO[] allWeaponsArray;
    public ArmourSO[] allArmourArray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public int FindItemIndexInArray(string targetArray, string itemName)
    {
        switch (targetArray)
        {
            case "Weapons":

                foreach (WeaponsSO weapon in allWeaponsArray)
                {
                    if (weapon.ToString() == itemName)
                    {
                        int index = System.Array.IndexOf(allWeaponsArray, weapon);
                        return index;
                    }
                }
                
                return 1000;

            case "Armour":
                    foreach (ArmourSO armour in allArmourArray)
                    {
                        if (armour.ToString() == itemName)
                        {
                            int index = System.Array.IndexOf(allArmourArray, armour);
                            return index;
                        }
                    }

                return 1000;

            default:

                return 1000;

                
        }
    }*/
}
