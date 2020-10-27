using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveSO : ScriptableObject
{
    public string waveName;
    public int waveCount;
    public GameObject[] enemies;
}
