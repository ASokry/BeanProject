using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGravitySystem : MonoBehaviour
{
    public static InventoryGravitySystem Instance { get; private set; }

    public enum GravitySystemState { Selecting, Starting, Processing, Finalizing, Reseting }
    [SerializeField] private GravitySystemState currentState = GravitySystemState.Selecting;

    [SerializeField] private List<InventoryGravity> gravityList = new List<InventoryGravity>();
    private InventoryGravity currentInventoryGravity;

    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(InventoryGravity inventoryGravity)
    {
        //print("add");
        gravityList.Add(inventoryGravity);
    }

    public void RemoveFromList(int index)
    {
        //print("remove");
        gravityList.RemoveAt(index);
        currentState = GravitySystemState.Selecting;
    }

    private void Update()
    {
        if (currentState == GravitySystemState.Selecting && gravityList.Count > 0)
        {
            currentInventoryGravity = gravityList[0];
            currentState = GravitySystemState.Processing;
        }

        if (currentState == GravitySystemState.Processing && gravityList.Count > 0)
        {
            //Gravity(currentInventoryGravity);
        }

        foreach (InventoryGravity inventoryGravity in gravityList)
        {
            if (inventoryGravity.GetGravityState())
            {
                Gravity(inventoryGravity);
            }
        }
    }

    private void Gravity(InventoryGravity inventoryGravity)
    {
        PlacedObject placedObject = inventoryGravity.GetPlacedObject();
        PlacedObjectTypeSO.Dir dir = placedObject.GetDir();
        InventoryTetris inventoryTetris = inventoryGravity.GetInventoryTetris();
        Vector2Int rowBelow = inventoryGravity.GetRowBelow();

        List<Vector2Int> gridPositionList = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(rowBelow, dir);
        bool canPlace = inventoryTetris.CheckBuildItemPositions(gridPositionList, placedObject);
        print(canPlace);

        /*if (canPlace)
        {
            inventoryTetris.RemoveItemAt(placedObject.GetGridPosition());
            inventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, rowBelow, dir);
        }*/
    }
}
