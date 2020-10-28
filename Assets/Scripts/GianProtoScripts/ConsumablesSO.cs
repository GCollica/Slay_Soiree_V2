using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class ConsumablesSO : ScriptableObject
{
    public string consumableName;
    public Sprite consumableSprite;
    public int cost;
}
