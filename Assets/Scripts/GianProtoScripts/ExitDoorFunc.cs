using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorFunc : MonoBehaviour
{
    private Collider2D interactCollider;

    public void EnableCollider()
    {
        interactCollider = this.gameObject.GetComponent<BoxCollider2D>();

        interactCollider.enabled = true;
        Debug.Log("Enabled Collider");
    }
}
