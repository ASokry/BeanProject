using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    public enum ItemType { Weapon, Consumable, None };
    [SerializeField] private ItemType itemType;

    public string GetName() { return itemName; }
    public ItemType GetItemType() { return itemType; }

    public virtual void DestroyThisItemOnGrid(InventoryTetris inventoryTetris, Vector2Int cooridnate)
    {
        //Method for destroying the consumable
        print("Destroying this item");
        inventoryTetris.RemoveItemAt(cooridnate);
    }
}
