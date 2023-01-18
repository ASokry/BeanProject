using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : InventoryConsumable
{
    private int charges = 1;

    public override void Use()
    {
        base.Use();
        if (charges > 0)
        {
            print("Using Medkit");
            charges--;
        }
    }

    public override void DestroyThisConsumable()
    {
        base.DestroyThisConsumable();
        if (charges <= 0)
        {
            Destroy(this);
        }
        else
        {
            print("Still has charges, cannot be destroyed");
        }
    }
}
