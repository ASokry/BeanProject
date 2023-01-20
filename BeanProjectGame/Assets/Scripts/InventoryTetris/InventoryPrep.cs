using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPrep : MonoBehaviour
{
    public static InventoryPrep Instance { get; private set; }

    [SerializeField] private List<InventoryTetris> inventoriesToPrep = new List<InventoryTetris>();
    [SerializeField] private List<ItemTetrisSO> requiredItems = new List<ItemTetrisSO>();
    private List<PlacedObjectTypeSO> copyOfItemContainerItems = new List<PlacedObjectTypeSO>();

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            //print(IsInventoryPrepped(0));
        }
    }

    public bool IsInventoryPrepped(int index)
    {
        if (inventoriesToPrep.Count <= 0)
        {
            print("There are inventories to prep");
            return false;
        }

        if(index < 0 || index > inventoriesToPrep.Count)
        {
            print("index is negative or too high");
            return false;
        }

        InventoryTetris inventoryTetris = inventoriesToPrep[index];
        return CheckRequirements(inventoryTetris);
    }

    private bool CheckRequirements(InventoryTetris inventoryTetris)
    {
        if (requiredItems.Count <= 0) { return true; }

        RectTransform itemContainer = inventoryTetris.GetItemContainer();
        PlacedObject[] items = itemContainer.transform.GetComponentsInChildren<PlacedObject>();
        foreach(PlacedObject placedObject in items) { copyOfItemContainerItems.Add(placedObject.GetPlacedObjectTypeSO()); }

        int count = 0;
        foreach (ItemTetrisSO item in requiredItems)
        {
            //print(copyOfItemContainerItems.Contains(item));
            if (copyOfItemContainerItems.Contains(item))
            {
                copyOfItemContainerItems.Remove(item);
                count++;
            }
        }
        copyOfItemContainerItems.Clear();
        //print(count);
        if (count == requiredItems.Count)
        {
            return true;
        }
        return false;
    }
}
