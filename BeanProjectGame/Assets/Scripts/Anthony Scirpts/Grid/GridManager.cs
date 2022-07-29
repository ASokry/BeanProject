using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// helps with moving items on grid and from one gird to another grid
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private List<GridObject> gridObjectList;
    private Grid<GridCellValue> currentGridMouseIsIn;
    private float gridsCellSize = 0;

    [SerializeField] private ItemObject itemOnMouse; private bool once = false;
    private ItemObject.Dir itemDirection = ItemObject.Dir.Down;
    private PlacedGridObject originalPlacedGridObject;
    private List<Vector2Int> originalPlacedGridObjectCoordinates;
    private Grid<GridCellValue> originalPlacedGridObjectGrid;

    private GridCellValue gridCellValue;
    private PlacedGridObject placedGridObject;

    private GameObject ghostObject;
    private bool ghostFollow = false;
    private int ghostID;

    private bool canPlace = false;
    private bool inventoryClear = false;

    private void Awake()
    {
        Instance = this;

        if (gridObjectList.Count <= 0)
            Debug.LogError(this + ": list of Grid Objects is empty." + "gridObjectList.Count: " + gridObjectList.Count);
    }

    private void Start()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            gridObject.AwakeScirpt();
            //print(1);
        }
    }

    private void Update()
    {
        GetCurrentGridMouseIsIn();
        //print(currentGridMouseIsIn.GetParent().name);
        MouseClickOnItem();
        MouseReleaseWithItem();
    }

    private void GetCurrentGridMouseIsIn()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            if (gridObject.IsMouseInThisGrid())
            {
                currentGridMouseIsIn = gridObject.GetGrid();
                //currentGridMouseIsIn.GetXYPosition(Input.mousePosition, out int x, out int y);
                break;
            }

            //print("no grid");
            currentGridMouseIsIn = null;
        }

        if (!once && currentGridMouseIsIn != null)
        {
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, new Vector2Int(0, 1), ItemObject.Dir.Down);
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, new Vector2Int(2, 1), ItemObject.Dir.Down);
            itemOnMouse = null;
            once = true;
            //print("done");
        }
    }

    private void MouseClickOnItem()
    {
        if (Input.GetMouseButton(0))
        {
            if (!itemOnMouse && currentGridMouseIsIn != null)
            {
                gridsCellSize = currentGridMouseIsIn.GetCellSize();
                gridCellValue = currentGridMouseIsIn.GetGridCellValue(Input.mousePosition);
                placedGridObject = gridCellValue.GetPlacedGridObject();
                print("get placed object");
            }
            
            if (placedGridObject != null && placedGridObject.GetItemType() != ItemObject.itemTyp.Null)
            {
                ghostFollow = true;

                itemOnMouse = placedGridObject.GetComponent<ItemObject>();

                SetOriginalPlacedGridObject();
            }

            if (Input.GetMouseButtonDown(1) && itemOnMouse)
            { 
                itemDirection = ItemObject.GetNextDir(itemDirection); 
            }

            CreateGhost();
            GhostTracking();
            //LowerOpacityOriginalPlacedGridObject(true);
        }
        /*else if (Input.GetMouseButton(0) && itemOnMouse != null)
        {
            ClearItemOnMouse();
            ClearGhost();
        }*/
    }

    private void SetOriginalPlacedGridObject()
    {
        if (originalPlacedGridObject == null && originalPlacedGridObjectCoordinates==null && originalPlacedGridObjectGrid==null)
        {
            originalPlacedGridObject = placedGridObject;
            originalPlacedGridObjectCoordinates = originalPlacedGridObject.GetGridPositionList();
            originalPlacedGridObjectGrid = currentGridMouseIsIn;
        }
    }

    private void LowerOpacityOriginalPlacedGridObject(bool fade)
    {
        if (fade && originalPlacedGridObject != null)
        {
            Color lowOpacityColor = originalPlacedGridObject.GetComponentInChildren<SpriteRenderer>().color;
            lowOpacityColor.a = 200;
            originalPlacedGridObject.GetComponentInChildren<SpriteRenderer>().color = lowOpacityColor;
        }
        else
        {
            Color lowOpacityColor = originalPlacedGridObject.GetComponentInChildren<SpriteRenderer>().color;
            lowOpacityColor.a = 255;
            originalPlacedGridObject.GetComponentInChildren<SpriteRenderer>().color = lowOpacityColor;
        }
    }

    private void MouseReleaseWithItem()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentGridMouseIsIn != null && itemOnMouse != null)
            {
                currentGridMouseIsIn.GetXYPosition(Input.mousePosition, out int x, out int y);
                //print(currentGridMouseIsIn.GetParent().name + ": " + x + ", " + y);

                List<Vector2Int> gridCoordinatesList = itemOnMouse.GetCoordinateList(new Vector2Int(x, y), itemDirection);

                CheckCoordinatesOnGrid(gridCoordinatesList);
                //print(canPlace);
                PlaceObjectDown(new Vector2Int(x, y));
            }
            else if(itemOnMouse != null)
            {
                ResetItemOnMouse();
                ClearGhost();
            }
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
                print("not in grid");
                canPlace = false;
                break;
            }

            // if coordinates already contains a DIFFERENT placed object, then we can't place down an object
            if (!currentGridMouseIsIn.GetGridCellValue(coordinates.x, coordinates.y).IsPlacedGridObjectEmpty() && currentGridMouseIsIn.GetGridCellValue(coordinates.x, coordinates.y).GetPlacedGridObjectItemID() != ghostID)
            {
                print("taken");
                canPlace = false;
                break;
            }
        }
    }

    private void PlaceObjectDown(Vector2Int itemObjPos)
    {
        if (canPlace)
        {
            DestroyOriginalPlacedGridObject();
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, itemObjPos, itemDirection);
            ResetItemOnMouse();
            ClearGhost();
        }
        else
        {
            ResetItemOnMouse();
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
            ghostID = itemOnMouse.GetComponent<PlacedGridObject>().GetItemID();
        }
    }

    private void GhostTracking()
    {
        if (ghostFollow && ghostObject != null)
        {
            ghostObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, itemOnMouse.GetRotationAngle(itemDirection));
            ghostObject.GetComponent<Transform>().position = Input.mousePosition + itemOnMouse.GetPositionOffset(itemDirection, gridsCellSize);
        }
        else
        {
            ResetItemOnMouse();
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

    private void ResetItemOnMouse()
    {
        if (once)
        {
            itemOnMouse = null;
            canPlace = false;
            placedGridObject = null;
            //print("clear");
        }
    }

    private void DestroyOriginalPlacedGridObject()
    {
        originalPlacedGridObject.DestroySelf();
        //print(originaloriginalPlacedGridObjectGrid.GetParent().name);
        //print(itemOnMouse==null);
        foreach (Vector2Int originalCoordinates in originalPlacedGridObjectCoordinates)
        {
            originalPlacedGridObjectGrid.GetGridCellValue(originalCoordinates.x, originalCoordinates.y).ClearPlacedGridObject();
        }
        ResetOriginalGridObjects();
    }

    private void ResetOriginalGridObjects()
    {
        //LowerOpacityOriginalPlacedGridObject(false);
        originalPlacedGridObjectGrid = null;
        originalPlacedGridObjectCoordinates = null;
        originalPlacedGridObject = null;
    }

    public void SpawnItemInGrid(Grid<GridCellValue> theGrid, ItemObject itemObj, Vector2Int itemObjPos, ItemObject.Dir dir)
    {
        Vector2Int rotationOffset = itemObj.GetRotationOffset(dir);

        Vector3 itemPosition = theGrid.GetWorldPosition(itemObjPos.x, itemObjPos.y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * theGrid.GetCellSize();

        PlacedGridObject item = PlacedGridObject.Create(itemPosition, new Vector2Int(itemObjPos.x, itemObjPos.y), dir, itemObj, theGrid.GetParent());

        theGrid.GetGridCellValue(itemObjPos.x, itemObjPos.y).SetPlacedGridObject(item);

        //Corner case check for bug where image is turned off when original placedObject is destroyed
        if (!item.GetComponentInChildren<Image>().enabled) { item.GetComponentInChildren<Image>().enabled = true; }

        List<Vector2Int> itemPositionList = itemObj.GetCoordinateList(new Vector2Int(itemObjPos.x, itemObjPos.y), dir);
        foreach (Vector2Int gridPosition in itemPositionList)
        {
            theGrid.GetGridCellValue(gridPosition.x, gridPosition.y).SetPlacedGridObject(item);
        }
    }
}
