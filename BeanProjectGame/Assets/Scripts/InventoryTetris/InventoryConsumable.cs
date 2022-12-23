using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryConsumable : InventoryItem
{
    //Dummy Script, will be rewritten after further testing item memory
    private int count = 3;

    private void Update()
    {
        Use();
        PrintText();
    }

    public virtual void Use()
    {
        //Use Consumable
        if (Input.GetKeyDown(KeyCode.U))
        {
            count--;
        }
    }

    public void PrintText()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            print(gameObject.GetInstanceID() + ": " + count);
        }
    }
}
