using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedKit : InventoryConsumable
{
    [SerializeField] private int healAmount;

    public override void Use()
    {
        base.Use();
        print("Using Medkit");
    }

    public override int GetHealAmount()
    {
        base.GetHealAmount();
        return healAmount;//cannot be used due to parent/sub class referencing
    }

    /*public override void DestroyThisItem()
    {
        base.DestroyThisItem();
        if (charges <= 0)
        {
            Destroy(this);
        }
        else
        {
            print("Still has charges, cannot be destroyed");
        }
    }*/
}
