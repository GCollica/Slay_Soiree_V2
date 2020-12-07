using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuirkSelection : MonoBehaviour
{
    public QuirkPickerUI quirkPickerUI;

    public GameObject quirkCard;

    private Image highlight;

    public int index;

    private void Awake()
    {
        highlight = quirkCard.GetComponent<Image>();
        highlight.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Card Highlighted");
            highlight.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Card Highlighted");
            highlight.enabled = false;
        }
    }

    public void ActivateQuirk()
    {
        //Debug.Log("Quirk Activated");

        switch (index)
        {
            case 1:
                Debug.Log("Quirk 1");
                quirkPickerUI.PickQuirk1Button();
                break;
            case 2:
                quirkPickerUI.PickQuirk2Button();
                break;
            case 3:
                quirkPickerUI.PickQuirk3Button();
                break;
        }
    }
}
