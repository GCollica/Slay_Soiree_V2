using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemPedistool : MonoBehaviour
{
    [HideInInspector]
    public enum ItemType {Empty, Weapon, Armour, Consumable};
    [Header("For reference only, don't touch.")]
    public ItemType currentItemType = ItemType.Empty;    
    public WeaponsSO currentWeaponItem;
    public ArmourSO currentArmourItem;
    public ConsumablesSO currentConsumableItem;
    [Space(5)]
    [Header("U.I. references, edit in the prefab.")]
    public SpriteRenderer itemSpriteRenderer;
    public TMPro.TMP_Text displayedText;
    private ItemManager itemManager;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    // Start is called before the first frame update
    void Awake()
    {
        itemManager = FindObjectOfType<ItemManager>();
        ChooseItemType();
        ChooseRandomItem();
        SetUIElements();
        displayedText.canvasRenderer.SetAlpha(0);
    }

    private void ChooseItemType()
    {
        int chosenType = Random.Range(0, 4);

        if(chosenType == 0)
        {
            currentItemType = ItemType.Weapon;
        }
        else if(chosenType == 1)
        {
            currentItemType = ItemType.Armour;
        }
        else if(chosenType == 2 || chosenType == 3)
        {
            currentItemType = ItemType.Consumable;
        }
    }

    private void ChooseRandomItem()
    {
        switch (currentItemType)
        {
            case ItemType.Weapon:
                int chosenWeaponIndex = Random.Range(0, itemManager.allWeaponsArray.Length);
                currentWeaponItem = itemManager.allWeaponsArray[chosenWeaponIndex];
                break;

            case ItemType.Armour:
                int chosenArmourIndex = Random.Range(0, itemManager.allArmourArray.Length);
                currentArmourItem = itemManager.allArmourArray[chosenArmourIndex];
                break;

            case ItemType.Consumable:
                int chosenConsumableIndex = Random.Range(0, itemManager.allConsumablesArray.Length);
                currentConsumableItem = itemManager.allConsumablesArray[chosenConsumableIndex];
                break;

            default:
                break;
        }
    }

    public void SetUIElements()
    {
        switch (currentItemType)
        {
            case ItemType.Weapon:
                itemSpriteRenderer.sprite = currentWeaponItem.weaponSprite;
                displayedText.text = "Buy " + currentWeaponItem.weaponName + " for " + currentWeaponItem.cost.ToString() + " gold.";
                break;

            case ItemType.Armour:
                itemSpriteRenderer.sprite = currentArmourItem.armourSprite;
                displayedText.text = "Buy " + currentArmourItem.armourName + " for " + currentArmourItem.cost.ToString() + " gold";
                break;

            case ItemType.Consumable:
                itemSpriteRenderer.sprite = currentConsumableItem.consumableSprite;
                displayedText.text = "Buy " + currentConsumableItem.consumableName + " for " + currentConsumableItem.cost.ToString() + " gold";
                break;

            case ItemType.Empty:
                itemSpriteRenderer.sprite = null;
                displayedText.text = null;
                break;
        }
    }

    public void ClearPedistool()
    {
        currentItemType = ItemType.Empty;
        currentArmourItem = null;
        currentWeaponItem = null;
        currentConsumableItem = null;
        SetUIElements();
    }

    IEnumerator FadeInIEnumerator()
    {
        isFadingIn = true;

        for (float targetAlpha = displayedText.canvasRenderer.GetAlpha(); targetAlpha < 1.1; targetAlpha += 0.1f)
        {
            displayedText.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.05f);
        }

        isFadingIn = false;
        StopCoroutine(nameof(FadeInIEnumerator));
    }

    IEnumerator FadeOutIEnumerator()
    {
        isFadingOut = true;

        for (float targetAlpha = displayedText.canvasRenderer.GetAlpha(); targetAlpha > -0.1; targetAlpha -= 0.1f)
        {
            displayedText.canvasRenderer.SetAlpha(targetAlpha);
            yield return new WaitForSeconds(0.05f);
        }
        
        isFadingOut = false;
        StopCoroutine(nameof(FadeInIEnumerator));
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(isFadingOut == true)
        {
            StopCoroutine(nameof(FadeOutIEnumerator));
            isFadingOut = false;
        }
        if(displayedText.canvasRenderer.GetAlpha() >= 0.4f)
        {
            return;
        }
        StartCoroutine(nameof(FadeInIEnumerator));
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(isFadingIn == true)
        {
            StopCoroutine(nameof(FadeOutIEnumerator));
            isFadingOut = true;
        }
        if(displayedText.canvasRenderer.GetAlpha() <= 0.4f)
        {
            return;
        }
        
        StartCoroutine(nameof(FadeOutIEnumerator));
    }
}
