using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuirkManager : MonoBehaviour
{
    public List<GameObject> Quirks;

    //public GameObject totem;

    void Start()
    {
        DamageTotem();
    }

    #region DamageTotem
    void DamageTotem()
    {
        Instantiate(Quirks.Find(obj => obj.name == "DamageTotem"), transform);
    }
    #endregion
}
