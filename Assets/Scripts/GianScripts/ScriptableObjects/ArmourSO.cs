using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Armour")]
public class ArmourSO : ScriptableObject
{
    public string armourName;

    public Sprite armourSprite;

    public int resistance;
    public float movementSpeed;
    public int cost;
}
