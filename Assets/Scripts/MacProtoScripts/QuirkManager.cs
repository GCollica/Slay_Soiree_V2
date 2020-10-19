using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public List<Quirk> AllQuirks;
    public Quirk CurrentQuirk;
    
    //public List<GameObject> Quirks;
    public GameObject totem;
    public bool spawnedTotem = false;

    private QuirkSpawner quirkSpawner;

    void Awake()
    {
        quirkSpawner = FindObjectOfType<QuirkSpawner>();
        SetCurrentQuirk(1);
    }

    private void Update()
    {
        switch (CurrentQuirk.quirkID)
        {
            case 0:
                break;

            case 1:
                if(spawnedTotem != false)
                {
                    break;
                }
                quirkSpawner.SpawnQuirkObject(totem);
                spawnedTotem = true;
                break;

            default:
                break;
        }
    }

    public void SetCurrentQuirk(int quirkIDInput)
    {
        if(quirkIDInput == 0)
        {
            CurrentQuirk = null;
        }
        else
        {
            foreach (Quirk quirk in AllQuirks)
            {
                if (quirk.quirkID == quirkIDInput)
                {
                    CurrentQuirk = quirk;
                }
            }
        }        
    }

    #region DamageTotem
    void DamageTotem()
    {
        //Instantiate(Quirks.Find(obj => obj.name == "DamageTotem"), transform);
    }
    #endregion
}
