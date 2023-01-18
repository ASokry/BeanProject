using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryConsumable : InventoryItem
{
    // Indiviual consumable items will override virtual methods
    public virtual void Use()
    {
        //Use Consumable
        print("Using Consumable");
    }

    public virtual void DestroyThisConsumable()
    {
        //Method for destroying the consumable
        print("Destroying this Consumable");
        Destroy(this);
    }
}
