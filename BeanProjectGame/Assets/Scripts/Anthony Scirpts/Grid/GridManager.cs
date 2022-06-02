using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helps with moving items from gird to grid
public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GridObject> gridObjectList;
    private Grid<GridCellValue> currentGridMouseIsIn;

    [SerializeField] private ItemObject item;
    private ItemObject.Dir dir = ItemObject.Dir.Down;
    private GridCellValue gridValue;
    private PlacedGridObject placedGridObject;

    private Transform ghostObject;
    private bool ghostFollow = false;

    private bool canPlace = false;
    private bool inventoryClear = false;

    private void Awake()
    {
        if (gridObjectList.Count <= 0)
            Debug.LogError(this + ": list of Grid Objects is empty." + "gridObjectList.Count: " + gridObjectList.Count);
    }

    private void Start()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            gridObject.AwakeScirpt();
        }
    }

    private void Update()
    {
        GetCurrentGridMouseIsIn();
        MouseClick();
    }

    private void GetCurrentGridMouseIsIn()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            if (gridObject.IsMouseInThisGrid())
            {
                currentGridMouseIsIn = gridObject.GetGrid();
                //print(gridObject.GetGrid());
                break;
            }
        }
    }

    private void MouseClick()
    {
        if (Input.GetMouseButtonDown(0) && currentGridMouseIsIn != null)
        {
            print("click");
            SpawnItemInGrid(currentGridMouseIsIn, item, new Vector2Int(4,6), ItemObject.Dir.Down);
            //GridValue gridValue = currentGridMouseIsIn.GetGridValue(Input.mousePosition);
            //placedGridObject = gridValue.GetPlacedGridObject();
        }
    }

    private void SpawnItemInGrid(Grid<GridCellValue> theGrid, ItemObject itemObj, Vector2Int itemObjPos, ItemObject.Dir dir)
    {
        Vector2Int rotationOffset = itemObj.GetRotationOffset(dir);

        Vector3 itemPosition = theGrid.GetWorldPosition(itemObjPos.x, itemObjPos.y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * theGrid.GetCellSize();

        PlacedGridObject item = PlacedGridObject.Create(itemPosition, new Vector2Int(itemObjPos.x, itemObjPos.y), dir, itemObj, currentGridMouseIsIn.GetParent());

        theGrid.GetGridValue(itemObjPos.x, itemObjPos.y).SetPlacedGridObject(item);

        List<Vector2Int> firstItemPositionList = itemObj.GetCoordinateList(new Vector2Int(itemObjPos.x, itemObjPos.y), dir);
        foreach (Vector2Int gridPosition in firstItemPositionList)
        {
            theGrid.GetGridValue(gridPosition.x, gridPosition.y).SetPlacedGridObject(item);
        }
    }
}
