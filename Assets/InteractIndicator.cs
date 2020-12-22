using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    public GameObject InteractIndicatorGO;

    private void ToggleActive()
    {
        if(InteractIndicatorGO.activeInHierarchy == true)
        {
            InteractIndicatorGO.SetActive(false);
        }
        else
        {
            InteractIndicatorGO.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ExitDoor") || collision.gameObject.CompareTag("ItemPedistool") || collision.gameObject.CompareTag("QuirkCard"))
        {
            ToggleActive();
        }      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ExitDoor") || collision.gameObject.CompareTag("ItemPedistool") || collision.gameObject.CompareTag("QuirkCard"))
        {
            ToggleActive();
        }
    }
}
