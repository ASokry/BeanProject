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
