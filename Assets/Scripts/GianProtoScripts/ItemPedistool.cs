using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemPedistool : MonoBehaviour
{
    public enum ItemType {Empty, Weapon, Armour, Consumable};
    public ItemType currentItemType = ItemType.Empty;

    public WeaponsSO currentWeaponItem;
    public ArmourSO currentArmourItem;
    public ConsumablesSO currentConsumableItem;

    public GameObject currentitemSpriteGO;
    public Text currentItemCostText;

    private ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        ChooseItemType();
        ChooseRandomItem();
        SetUIElements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChooseItemType()
    {
        int chosenType = Random.Range(0, 3);

        if(chosenType == 0)
        {
            currentItemType = ItemType.Weapon;
        }
        else if(chosenType == 1)
        {
            currentItemType = ItemType.Armour;
        }
        else if(chosenType == 2)
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
                currentitemSpriteGO.GetComponent<SpriteRenderer>().sprite = currentWeaponItem.weaponSprite;
                currentItemCostText.text = "Buy " + currentWeaponItem.weaponName + " for " + currentWeaponItem.cost.ToString() + " gold.";
                break;

            case ItemType.Armour:
                currentitemSpriteGO.GetComponent<SpriteRenderer>().sprite = currentArmourItem.armourSprite;
                currentItemCostText.text = "Buy " + currentArmourItem.armourName + " for " + currentArmourItem.cost.ToString() + " gold";
                break;

            case ItemType.Consumable:
                currentitemSpriteGO.GetComponent<SpriteRenderer>().sprite = currentConsumableItem.consumableSprite;
                currentItemCostText.text = "Buy " + currentConsumableItem.consumableName + " for " + currentConsumableItem.cost.ToString() + " gold";
                break;

            case ItemType.Empty:
                currentitemSpriteGO.GetComponent<SpriteRenderer>().sprite = null;
                currentItemCostText.text = null;
                break;
        }
    }

    public void ClearPedistool()
    {
        currentItemType = ItemType.Empty;
        currentArmourItem = null;
        currentWeaponItem = null;
        SetUIElements();
    }
}
