using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryConsumable : InventoryItem
{
    public enum ConsumableType { Healing };
    [SerializeField] private ConsumableType consumableType;

    public ConsumableType GetConsumableType() { return consumableType; }

    // Indiviual consumable items will override virtual methods
    public virtual void Use()
    {
        //Use Consumable
        print("Using Consumable");
    }

    public virtual int GetHealAmount()
    {
        print("healing by 20");
        return 20; //Healing is temporily hardcoded for playtest
    }
}
