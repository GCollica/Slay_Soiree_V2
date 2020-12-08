using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorFunc : MonoBehaviour
{
    private Collider2D interactCollider;

    private RunHandler runHandler;

    private void Awake()
    {
        FindObjectOfType<SoundManager>().Play("Door Open");
        runHandler = FindObjectOfType<RunHandler>();
    }

    public void EnableCollider()
    {
        if(this.gameObject.GetComponent<BoxCollider2D>().enabled != true)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void ToggleExitDoorCollider(bool inputState)
    {
        if (this.gameObject.GetComponent<BoxCollider2D>().enabled != inputState)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = inputState;
        }
    }

    public void ExitRoom()
    {
        ToggleExitDoorCollider(false);
        runHandler.BeginNewRoomCoroutine();
    }
}
