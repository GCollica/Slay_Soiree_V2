using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public List<QuirkSO> AllQuirks;
    public QuirkSO CurrentQuirk;

    public List<QuirkSO> QuirkChoices;
    
    //Totem Gameobject, assign prefab in inspector.
    public GameObject totem;
    public bool spawnedTotem = false;

    public GameObject goldPouch;

    private QuirkSpawner quirkSpawner;
    private QuirkPickerUI quirkPickerUI;

    void Awake()
    {
        quirkSpawner = FindObjectOfType<QuirkSpawner>();
        quirkPickerUI = FindObjectOfType<QuirkPickerUI>();
    }

    private void Update()
    {
        if(CurrentQuirk == null)
        {
            return;
        }

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
                Invoke(nameof(SpawnTotem), 1.5f);
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
        spawnedTotem = false;

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

    public void ChoseQuirkChoices()
    {
        int beginingIndex = Random.Range(0, AllQuirks.Count - 3);
        List<QuirkSO> chosenQuirks = AllQuirks.GetRange(beginingIndex, 3);

        foreach (QuirkSO quirk in chosenQuirks)
        {
            QuirkChoices.Add(quirk);
        }

        quirkPickerUI.SetUIText();
        Debug.Log("Chose Quirks");
    }

    #region DamageTotem
    void SpawnTotem()
    {
        quirkSpawner.SpawnQuirkObject(totem);
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
        quirkSpawner.SpawnQuirkObjectTargetPos(goldPouch, sourcePlayer.transform.GetChild(3).transform);
    }
    #endregion
}
