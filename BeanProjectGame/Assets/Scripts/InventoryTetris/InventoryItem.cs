using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public enum ItemType { Weapon, Consumble, None };
    [SerializeField] private ItemType itemType;

    public ItemType GetItemType() { return itemType; }
}
