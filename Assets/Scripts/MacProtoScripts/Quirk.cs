using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quirk", menuName = "Quirk")]
public class Quirk : ScriptableObject
{
    public int quirkID;
    public string quirkName;
    public string quirkDescription;
    public Sprite quirkCardArt;
    public int quirkRarity;
}
