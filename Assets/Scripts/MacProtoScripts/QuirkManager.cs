using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public List<QuirkSO> AllQuirks;
    public QuirkSO CurrentQuirk;
    
    //Totem Gameobject, assign prefab in inspector.
    public GameObject totem;
    public bool spawnedTotem = false;

    public GameObject goldPouch;

    private QuirkSpawner quirkSpawner;

    void Awake()
    {
        quirkSpawner = FindObjectOfType<QuirkSpawner>();
        SetCurrentQuirk(Random.Range(1, 4));
    }

    private void Update()
    {
        switch (CurrentQuirk.quirkID)
        {
            case 0:
                break;
            //case 1 is Enemy 2x Damage Totem Quirk.
            case 1:
                if(spawnedTotem != false)
                {
                    break;
                }
                quirkSpawner.SpawnQuirkObject(totem);
                spawnedTotem = true;
                break;
            //Case 2 is MSRandomiser Quirk.
            case 2:
                break;
            //Case 3 is GoldGauntet Quirk.
            case 3:
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
            return;
        }
        else
        {
            foreach (QuirkSO quirk in AllQuirks)
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

    #region MSRandomiser
    public int RandomiseMSModifier()
    {
        int randomReturn = Random.Range(0, 2);
        return randomReturn;
    }
    #endregion

    #region GoldGauntlet
    public void SpawnGoldPouch(GameObject sourcePlayer)
    {
        Instantiate(goldPouch, sourcePlayer.transform.position, Quaternion.identity);
    }
    #endregion
}
