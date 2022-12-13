using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGravity : MonoBehaviour
{
    private InventoryTetris inventoryTetris;
    private PlacedObject placedObject;

    [SerializeField] private bool isGravity = true;

    public InventoryTetris GetInventoryTetris() { return inventoryTetris; }
    public PlacedObject GetPlacedObject() { return placedObject; }
    public bool IsGravityOn() { return isGravity; }
    public void ChangeGravity(bool b) { isGravity = b; }

    private void Awake()
    {
        placedObject = GetComponent<PlacedObject>();
    }

    public void Setup(InventoryTetris inventoryTetris)
    {
        this.inventoryTetris = inventoryTetris;
        AddToInventoryGravitySystem();
    }

    private void AddToInventoryGravitySystem()
    {
        InventoryGravitySystem.Instance.AddToList(this);
        if (isGravity)
        {
            InventoryGravitySystem.OnGravity += TryMoveDown;
        }
    }

    private void TryMoveDown()
    {
        PlacedObjectTypeSO.Dir dir = placedObject.GetDir();
        List<Vector2Int> gridPositionList = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(GetRowBelow(), dir);
        bool canPlace = inventoryTetris.CheckBuildItemPositions(gridPositionList, placedObject);
        //print(canPlace);
        if (canPlace && isGravity)
        {
            InventoryGravitySystem.Instance.AddToList(this);
        }
    }

    public Vector2Int GetRowBelow()
    {
        Vector2Int currentPosition = placedObject.GetGridPosition();
        PlacedObjectTypeSO.Dir dir = placedObject.GetDir();
        List<Vector2Int> currentPositionList = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(currentPosition,dir);
        int lowestPositionY = currentPosition.y;
        List<int> positionsBelowOrigin = new List<int>();

        foreach (Vector2Int position in currentPositionList)
        {
            if (currentPosition.y > position.y && !positionsBelowOrigin.Contains(position.y))
            {
                positionsBelowOrigin.Add(position.y);
            }

            if (lowestPositionY > position.y)
            {
                lowestPositionY = position.y;
            }
        }

        Vector2Int targetPosition = new Vector2Int(currentPosition.x, (lowestPositionY-1) + positionsBelowOrigin.Count);
        //print(targetPosition);
        return targetPosition;
    }

    private void OnDestroy()
    {
        InventoryGravitySystem.OnGravity -= TryMoveDown;
    }

    private void OnDisable()
    {
        InventoryGravitySystem.OnGravity -= TryMoveDown;
    }
}
