using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// helps with moving items on grid and from one gird to another grid
public class GridManager : MonoBehaviour
{
    public delegate void ItemSpawnInGridAction();
    public event ItemSpawnInGridAction OnItemSpawnedInGrid;
    public static GridManager Instance { get; private set; }

    [SerializeField] private List<GridObject> gridObjectList;
    private Grid<GridCellValue> currentGridMouseIsIn;
    private GridObject currentGridObject;
    private float gridsCellSize = 0;

    [SerializeField] private ItemObject itemOnMouse;
    private ItemObject.Dir managerItemDirection = ItemObject.Dir.Down;

    private PlacedGridObject originalPlacedGridObject;
    private List<Vector2Int> originalPlacedGridObjectCoordinates;
    private Grid<GridCellValue> originalPlacedGridObjectGrid;

    private GridCellValue gridCellValue;
    private PlacedGridObject placedGridObject;

    private GameObject ghostObject;
    private bool ghostFollow = false;
    private int ghostID;

    [Range(0,1)]
    [SerializeField] private float lowOpacityColor = 0.5f;

    private bool canPlace = false;
    private bool inventoryClear = false;

    [SerializeField] private bool searchMode = false;
    [SerializeField] private float searchDelay = 1f;
    [SerializeField] private string targetItemName = "H-Ammo"; //Update this to specific item name based on items in slot
    public ItemObject foundedItem;
    public GridCoordinate foundedItemCoordinates;

    private CharacterMotion characterMotion;

    public class GridCoordinate
    {
        public GridObject grid;
        public int x;
        public int y;

        public GridCoordinate(GridObject grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
    }

    private int searchCounter = 0;

    private Canvas gridCanvas;
    private Camera gridCamera;

    private void Awake()
    {
        Instance = this;

        if (gridObjectList.Count <= 0)
            Debug.LogError(this + ": list of Grid Objects is empty." + "gridObjectList.Count: " + gridObjectList.Count);

        gridCanvas = GameObject.FindGameObjectWithTag("GridCanvas").GetComponent<Canvas>();
        gridCamera = GameObject.FindGameObjectWithTag("GridCamera").GetComponent<Camera>();

        if (gridCanvas == null || gridCamera == null)
            Debug.LogError(this + ": missing reference to canvas or camera!");
    }

    private void Start()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            if(gridObject.gameObject.activeSelf)
                gridObject.AwakeScirpt();
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            characterMotion = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMotion>();
        }
        else
        {
            Debug.LogError(this + ": cannot find Player Object that has CharacterMotion component");
        }
    }

    private void Update()
    {
        GetCurrentGridMouseIsIn();
        //print(currentGridMouseIsIn.GetParent().name);
        MouseClickOnItem();
        ChangeItemOnMouseDirection();
        MouseReleaseWithItem();

        //StartGridTraversal();
    }

    private void GetCurrentGridMouseIsIn()
    {
        foreach (GridObject gridObject in gridObjectList)
        {
            if (gridObject.IsMouseInThisGrid())
            {
                currentGridMouseIsIn = gridObject.GetGrid();
                //currentGridMouseIsIn.GetXYPosition(Input.mousePosition, out int x, out int y);
                currentGridObject = gridObject;
                break;
            }

            //print("no grid");
            currentGridMouseIsIn = null;
        }
    }

    private void MouseClickOnItem()
    {
        if (Input.GetMouseButton(0))
        {
            //print(currentGridMouseIsIn.GetParent().name + ": " + currentGridMouseIsIn.GetGridCellValue(Input.mousePosition));
            if (!itemOnMouse && currentGridMouseIsIn != null)
            {
                gridsCellSize = currentGridMouseIsIn.GetCellSize();
                gridCellValue = currentGridMouseIsIn.GetGridCellValue(Input.mousePosition);
                placedGridObject = gridCellValue.GetPlacedGridObject();
                //print(placedGridObject);
                //print("get placed object");
            }
            
            if (placedGridObject != null && placedGridObject.GetItemType() != ItemObject.ItemType.None && !itemOnMouse)
            {
                ghostFollow = true;

                itemOnMouse = placedGridObject.GetComponent<ItemObject>();
                //print(itemOnMouse);
                managerItemDirection = placedGridObject.GetDir();
                SetOriginalPlacedGridObject();
            }

            CreateGhost();
            GhostTracking();
        }
        else if (Input.GetMouseButtonDown(1) && !itemOnMouse)
        {
            print("right click");
            if (currentGridMouseIsIn != null)
            {
                gridCellValue = currentGridMouseIsIn.GetGridCellValue(Input.mousePosition);
                placedGridObject = gridCellValue.GetPlacedGridObject();
            }

            if (placedGridObject.GetComponent<WeaponObject>() != null)
            {
                characterMotion.SetWeaponObject(placedGridObject.GetComponent<WeaponObject>());
                //print("got it");
            }
            /*else if (placedGridObject.GetComponent<ConsumeObject>() != null)
            {
                characterMotion.SetWeaponObject(placedGridObject.GetComponent<ConsumeObject>());
                //print("got it");
            }*/

            gridCellValue = null;
            placedGridObject = null;
        }
    }

    private void ChangeItemOnMouseDirection()
    {
        if (Input.GetMouseButtonDown(1) && itemOnMouse)
        {
            //print("click");
            managerItemDirection = ItemObject.GetNextDir(managerItemDirection);
        }
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

    private void SetOpacityOriginalPlacedGridObject(float opacity)
    {
        if (originalPlacedGridObject != null)
        {
            Color newColor = originalPlacedGridObject.GetComponentInChildren<Image>().color;
            newColor.a = opacity;
            originalPlacedGridObject.GetComponentInChildren<Image>().color = newColor;
            //print(originalPlacedGridObject.GetComponentInChildren<Image>().color.a);
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

                List<Vector2Int> gridCoordinatesList = itemOnMouse.GetCoordinateList(new Vector2Int(x, y), managerItemDirection);

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
            //print(itemOnMouse);
            SpawnItemInGrid(currentGridMouseIsIn, itemOnMouse, itemObjPos, managerItemDirection, gridCanvas);
            ResetItemOnMouse();
            ClearGhost();

            //MoveGridArrow(currentGridObject, itemObjPos);
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
            Vector3 mousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCanvas.GetComponent<RectTransform>(), Input.mousePosition, gridCamera, out mousePos);
            ghostObject = Instantiate(itemOnMouse.GetPrefab(), mousePos, Quaternion.Euler(0,0,itemOnMouse.GetRotationAngle(managerItemDirection)));
            ghostObject.transform.localScale *= gridCanvas.transform.localScale.x;
            //print(ghostObject.transform.localScale);
            ghostObject.gameObject.name = itemOnMouse.GetItemName() + "(ghost)";
            ghostObject.transform.SetParent(CanvasMouse.Instance.gameObject.GetComponent<Transform>());
            ghostID = itemOnMouse.GetComponent<PlacedGridObject>().GetItemID();
            SetOpacityOriginalPlacedGridObject(lowOpacityColor);
        }
    }

    private void GhostTracking()
    {
        if (ghostFollow && ghostObject != null)
        {
            Vector3 mousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCanvas.GetComponent<RectTransform>(), Input.mousePosition, gridCamera, out mousePos);
            ghostObject.transform.rotation = Quaternion.Euler(0, 0, itemOnMouse.GetRotationAngle(managerItemDirection));
            //ghostObject.GetComponent<Transform>().position = Input.mousePosition + itemOnMouse.GetPositionOffset(managerItemDirection, gridsCellSize);
            ghostObject.transform.position = mousePos + itemOnMouse.GetPositionOffset(managerItemDirection, gridsCellSize) * gridCanvas.transform.localScale.x;
            //print(ghostObject.GetComponent<RectTransform>().position);
        }
        /*else
        {
            ResetItemOnMouse();
            ClearGhost();
        }*/
    }

    private void ClearGhost()
    {
        if (ghostObject != null)
        {
            ghostObject.gameObject.GetComponent<PlacedGridObject>().DestroySelf();
            ghostObject = null;
            ghostFollow = false;
            SetOpacityOriginalPlacedGridObject(1f);
        }
    }

    private void ResetItemOnMouse()
    {
        itemOnMouse = null;
        canPlace = false;
        placedGridObject = null;
        //print("clear");
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
        originalPlacedGridObjectGrid = null;
        originalPlacedGridObjectCoordinates = null;
        originalPlacedGridObject = null;
    }

    public void SpawnItemInGrid(Grid<GridCellValue> theGrid, ItemObject itemObj, Vector2Int itemObjPos, ItemObject.Dir dir, Canvas canvas)
    {
        //print(itemObj);
        Vector2Int rotationOffset = itemObj.GetRotationOffset(dir);

        //Vector3 itemPosition = theGrid.GetWorldPosition(itemObjPos.x, itemObjPos.y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * theGrid.GetCellSize();
        Vector3 itemPosition = theGrid.GetCanvasWorldPosition(canvas, itemObjPos.x, itemObjPos.y, gridCamera) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * theGrid.GetCellSize() * canvas.transform.localScale.x;

        float canvasScale = canvas.transform.localScale.x;
        PlacedGridObject item = PlacedGridObject.Create(itemPosition, new Vector2Int(itemObjPos.x, itemObjPos.y), dir, itemObj, theGrid.GetParent(), canvasScale);

        theGrid.GetGridCellValue(itemObjPos.x, itemObjPos.y).SetPlacedGridObject(item);

        //Corner case check for bug where image is turned off when original placedObject is destroyed
        if (item.GetComponentInChildren<Image>() != null) 
        {
            CheckImageProperties(item.GetComponentInChildren<Image>());
        }

        List<Vector2Int> itemPositionList = itemObj.GetCoordinateList(new Vector2Int(itemObjPos.x, itemObjPos.y), dir);
        foreach (Vector2Int gridPosition in itemPositionList)
        {
            //print(gridPosition.x + ", " + gridPosition.y);
            theGrid.GetGridCellValue(gridPosition.x, gridPosition.y).SetPlacedGridObject(item);
        }

        item.StartItemGravity();
        OnItemSpawnedInGrid();
    }

    private void CheckImageProperties(Image image)
    {
        image.GetComponentInChildren<Image>().enabled = true;
        Color fullOpacityColor = image.GetComponentInChildren<Image>().color;
        fullOpacityColor.a = 1f;
        image.GetComponentInChildren<Image>().color = fullOpacityColor;
    }

    public void StartGridTraversal(string target)
    {
        //if (searchMode && (target != null || target != ""))
        if ((target != null || target != ""))
        {
            searchMode = true;
            targetItemName = target;
            //print("start grid traversal");
            StartCoroutine(GridTraversal(gridObjectList[searchCounter]));
            /*for (int gridNum =0; gridNum<gridObjectList.Count; gridNum++)
            {
                StartCoroutine(GridTraversal(gridObjectList[gridNum]));
            }
            StopCoroutine("GridTraversal");*/
        }
    }

    private IEnumerator GridTraversal(GridObject gridToTraverse)
    {
        if (gridToTraverse.GetIsSearchable())
        {
            // Traverse through entire grid, starting at the top row
            int row = GetRowOfTopMostItem(gridToTraverse);
            //print(gridToTraverse.gameObject.name + ", " + row);
            while (row >= 0 && foundedItem == null && searchMode)
            {
                //Reveal and move the arrow along y axis of grid on left hand side
                MoveGridArrow(gridToTraverse, new Vector2Int(0, row));

                // search through each column of current row
                for (int column = 0; column < gridToTraverse.GetGridWidth(); column++)
                {
                    yield return new WaitForSeconds(searchDelay);

                    if (gridToTraverse.GetGrid().GetGridCellValue(column, row).IsPlacedGridObjectEmpty())
                    {
                        //print(gridToTraverse.GetGrid().GetGridCellValue(column, row));
                        continue; //if the placedGridObject is empty, then continue loop
                    }

                    if (CheckTargetItemName(gridToTraverse, column, row))
                    {
                        //if we found a matching targetItemname, then use the item
                        //print("found it at: " + column + ", " + row);
                        foundedItem = gridToTraverse.GetGrid().GetGridCellValue(column, row).GetPlacedGridObject().GetItemObject();
                        foundedItemCoordinates = new GridCoordinate(gridToTraverse, column, row);
                        //print(itemToUse);
                        searchMode = false;
                        gridToTraverse.HideArrow();
                        break;
                    }
                }

                yield return new WaitForSeconds(searchDelay);
                // at the end of the row, hide the arrow again
                gridToTraverse.HideArrow();

                //increment row counter, so we can go down to next row
                row--;
            }
        }

        //If there is another grid, continue traversal on next grid
        ContinueGridTraversal();
    }

    private void ContinueGridTraversal()
    {
        searchCounter++;
        if (searchCounter < gridObjectList.Count)
        {
            StartCoroutine(GridTraversal(gridObjectList[searchCounter]));
        }
        else
        {
            searchCounter = 0;
            searchMode = false;
        }
    }

    private int GetRowOfTopMostItem(GridObject grid)
    {
        //print(grid.GetGridWidth());
        for (int row = grid.GetGridHeight()-1; row >=0; row--)
        {
            //print(row);
            for (int col = 0; col<grid.GetGridWidth(); col++)
            {
                //print(col);
                if (!grid.GetGrid().GetGridCellValue(col,row).IsPlacedGridObjectEmpty())
                {
                    return row;
                }
            }
        }

        return -1;
    }

    private bool CheckTargetItemName(GridObject grid, int x, int y)
    {
        return grid.GetGrid().GetGridCellValue(x, y).GetPlacedGridObject().GetObjectName() == targetItemName;
    }

    public GridCoordinate GetFoundItemCoordinates()
    {
        return foundedItemCoordinates;
    }

    public void DestoryGridItem(GridCoordinate gridCoordinate)
    {
        PlacedGridObject objectToDestroy = gridCoordinate.grid.GetGrid().GetGridCellValue(gridCoordinate.x, gridCoordinate.y).GetPlacedGridObject();
        List<Vector2Int> CoordinatesToDestroy = objectToDestroy.GetGridPositionList();

        objectToDestroy.DestroySelf();
        foreach (Vector2Int coordinates in CoordinatesToDestroy)
        {
            gridCoordinate.grid.GetGrid().GetGridCellValue(coordinates.x, coordinates.y).ClearPlacedGridObject();
        }
    }

    private void MoveGridArrow(GridObject grid, Vector2Int tile)
    {
        grid.RevealArrow();
        //grid.MoveArrow(grid.GetGrid().GetWorldPosition(tile.x,tile.y));
        grid.MoveArrow(grid.GetGrid().GetCanvasWorldPosition(gridCanvas, tile.x, tile.y, gridCamera));
    }
}
