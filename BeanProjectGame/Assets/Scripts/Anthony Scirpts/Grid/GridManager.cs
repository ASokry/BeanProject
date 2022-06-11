using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helps with moving items on grid and from one gird to another grid
public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GridObject> gridObjectList;
    private Grid<GridCellValue> currentGridMouseIsIn;

    [SerializeField] private ItemObject itemOnMouse; private bool once = false;
    private ItemObject.Dir itemDirection = ItemObject.Dir.Down;
    private PlacedGridObject originalPlacedGridObject;
    private List<Vector2Int> originalPlacedGridObjectCoordinates;

    private GridCellValue gridCellValue;
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
        MouseClickOnItem();
        MouseReleaseWithItem();
    }

    private void GetCurrentGridMouseIsIn()
    {
        if (currentGridMouseIsIn == null)
        {
            foreach (GridObject gridObject in gridObjectList)
            {
                if (gridObject.IsMouseInThisGrid())
                {
                    currentGridMouseIsIn = gridObject.GetGrid();
                    //currentGridMouseIsIn.GetXYPosition(Input.mousePosition, out int x, out int y);
                    //print(x + ", " + y);
                    break;
                }
                else
                {
                    currentGridMouseIsIn = null;
                }
            }
        }

        if (!once && currentGridMouseIsIn != null)
        {
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, new Vector2Int(4, 6), ItemObject.Dir.Down);
            itemOnMouse = null;
            once = true;
            print("done");
        }
    }

    private void MouseClickOnItem()
    {
        if (Input.GetMouseButton(0) && currentGridMouseIsIn != null)
        {
            if (!itemOnMouse)
            {
                gridCellValue = currentGridMouseIsIn.GetGridCellValue(Input.mousePosition);
                placedGridObject = gridCellValue.GetPlacedGridObject();
            }

            if (placedGridObject != null && placedGridObject.GetItemType() != ItemObject.itemTyp.Null)
            {
                ghostFollow = true;

                itemOnMouse = placedGridObject.GetComponent<ItemObject>();

                originalPlacedGridObject = placedGridObject;
                originalPlacedGridObjectCoordinates = originalPlacedGridObject.GetGridPositionList();
            }

            if (Input.GetMouseButtonDown(1) && itemOnMouse != null) { itemDirection = ItemObject.GetNextDir(itemDirection); }

            CreateGhost();
            GhostTracking();
        }
        /*else if (Input.GetMouseButton(0) && itemOnMouse != null)
        {
            ClearItemOnMouse();
            ClearGhost();
        }*/
    }

    private void MouseReleaseWithItem()
    {
        if (Input.GetMouseButtonUp(0) && currentGridMouseIsIn != null && itemOnMouse != null)
        {
            currentGridMouseIsIn.GetXYPosition(Input.mousePosition, out int x, out int y);
            print(x + ", " + y);

            List<Vector2Int> gridCoordinatesList = itemOnMouse.GetCoordinateList(new Vector2Int(x, y), itemDirection);

            CheckCoordinatesOnGrid(gridCoordinatesList);
            PlaceObjectDown(new Vector2Int(x,y));
        }
    }

    private void CheckCoordinatesOnGrid(List<Vector2Int> coordinatesList)
    {
        canPlace = true;

        foreach (Vector2Int coordinates in coordinatesList)
        {
            // if coordinates are not on a grid then we can't place down an object
            if (currentGridMouseIsIn.GetGridCellValue(coordinates.x, coordinates.y) == null)
            {
                canPlace = false;
                break;
            }

            // if coordinates already contains a placed object, then we can't place down an object
            if (!currentGridMouseIsIn.GetGridCellValue(coordinates.x, coordinates.y).IsPlacedGridObjectEmpty())
            {
                canPlace = false;
                break;
            }
        }
    }

    private void PlaceObjectDown(Vector2Int itemObjPos)
    {
        if (canPlace)
        {
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, itemObjPos, itemDirection);
            ClearItemOnMouse();
            ClearGhost();
            DestroyOriginalPlacedGridObject();
        }
        else
        {
            ClearItemOnMouse();
            ClearGhost();
        }
    }

    private void CreateGhost()
    {
        if (ghostFollow && ghostObject == null)
        {
            ghostObject = Instantiate(itemOnMouse.GetPrefab(), Input.mousePosition, Quaternion.Euler(0,0,itemOnMouse.GetRotationAngle(itemDirection)));
            ghostObject.gameObject.name = itemOnMouse.nameString + "(ghost)";
            ghostObject.transform.SetParent(CanvasMouse.Instance.gameObject.GetComponent<Transform>());
        }
    }

    private void GhostTracking()
    {
        if (ghostFollow && ghostObject != null && currentGridMouseIsIn != null)
        {
            ghostObject.rotation = Quaternion.Euler(0, 0, itemOnMouse.GetRotationAngle(itemDirection));
            ghostObject.position = Input.mousePosition + itemOnMouse.GetPositionOffset(itemDirection, currentGridMouseIsIn.GetCellSize());
        }
        else
        {
            ClearItemOnMouse();
            ClearGhost();
        }
    }

    private void ClearGhost()
    {
        if (ghostObject != null && once)
        {
            ghostObject.gameObject.GetComponent<PlacedGridObject>().DestroySelf();
            ghostObject = null;
            ghostFollow = false;
        }
    }

    private void ClearItemOnMouse()
    {
        if (once)
        {
            itemOnMouse = null;
            print("clear");
        }
    }

    private void DestroyOriginalPlacedGridObject()
    {
        originalPlacedGridObject.DestroySelf();
        foreach (Vector2Int originalCoordinates in originalPlacedGridObjectCoordinates)
        {
            currentGridMouseIsIn.GetGridCellValue(originalCoordinates.x, originalCoordinates.y).ClearPlacedGridObject();
        }
        originalPlacedGridObjectCoordinates = null;
        originalPlacedGridObject = null;
    }

    private void SpawnItemInGrid(Grid<GridCellValue> theGrid, ItemObject itemObj, Vector2Int itemObjPos, ItemObject.Dir dir)
    {
        Vector2Int rotationOffset = itemObj.GetRotationOffset(dir);

        Vector3 itemPosition = theGrid.GetWorldPosition(itemObjPos.x, itemObjPos.y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * theGrid.GetCellSize();

        PlacedGridObject item = PlacedGridObject.Create(itemPosition, new Vector2Int(itemObjPos.x, itemObjPos.y), dir, itemObj, currentGridMouseIsIn.GetParent());

        theGrid.GetGridCellValue(itemObjPos.x, itemObjPos.y).SetPlacedGridObject(item);

        List<Vector2Int> itemPositionList = itemObj.GetCoordinateList(new Vector2Int(itemObjPos.x, itemObjPos.y), dir);
        foreach (Vector2Int gridPosition in itemPositionList)
        {
            theGrid.GetGridCellValue(gridPosition.x, gridPosition.y).SetPlacedGridObject(item);
        }
    }
}
