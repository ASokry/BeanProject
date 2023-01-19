using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryConsumable : InventoryItem
{
    public enum ConsumableType { Heal };

    // Indiviual consumable items will override virtual methods
    public virtual void Use()
    {
        //Use Consumable
        print("Using Consumable");
    }
}
