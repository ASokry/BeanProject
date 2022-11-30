using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGravity : MonoBehaviour
{
    private InventoryTetris inventoryTetris;
    private PlacedObject placedObject;
    private PlacedObjectTypeSO.Dir dir;

    private bool gravityState = false;

    public bool GetGravityState()
    {
        return gravityState;
    }

    private void Awake()
    {
        placedObject = GetComponent<PlacedObject>();
    }

    private void Start()
    {
        //InventoryGravitySystem.Instance.AddToList(this);
    }

    public void Setup(InventoryTetris inventoryTetris)
    {
        this.inventoryTetris = inventoryTetris;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Gravity();
        } 
    }

    // will need a gravity manager/systems
    public void Gravity()
    {
        dir = placedObject.GetDir();
        Vector2Int rowBelow = GetRowBelow();
        //print(rowBelow);

        //InventoryGravitySystem.Instance.RemoveFromList(0);

        // Remove item from its current inventory
        inventoryTetris.RemoveItemAt(placedObject.GetGridPosition());
        
        bool tryPlaceItem = inventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, rowBelow, dir);

        if (tryPlaceItem)
        {
            print("Item moved down");
            gravityState = true;
        }
        else
        {
            print("Item cannnot move down");
            gravityState = false;
            inventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, placedObject.GetGridPosition(), dir);
        }
    }

    private Vector2Int GetRowBelow()
    {
        Vector2Int currentPosition = placedObject.GetGridPosition();
        Vector2Int targetPosition = new Vector2Int(currentPosition.x, currentPosition.y-1);

        return targetPosition;
    }
}
