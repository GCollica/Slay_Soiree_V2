using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public List<Quirk> AllQuirks;
    public Quirk CurrentQuirk;
    
    //public List<GameObject> Quirks;
    //public GameObject totem;

    void Start()
    {
        SetCurrentQuirk(0);
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
