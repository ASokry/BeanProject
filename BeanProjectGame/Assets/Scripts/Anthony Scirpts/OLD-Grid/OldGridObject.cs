using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract Class for sciprts/Objects that uses a Grid
public class OldGridObject : MonoBehaviour
{
    private OldGrid<GridCellValue> grid;
    [SerializeField] private Transform gridParent;

    public enum GridType { BackpackMain, BackpackOther, Inventory, QuickSlot };

    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 100f; //Cellsize was intially 100, and equal to width/height of recttransform on Tile Image
    [SerializeField] private Transform startingPosition;
    [SerializeField] private GameObject gridTile;

    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowPadding;
    [SerializeField] private bool isSearchable = true;

    [SerializeField] private List<Vector2Int> nullCells;

    [SerializeField] private List<ItemObject> itemList;
    [SerializeField] private List<ItemObject.Dir> itemDirectionsList;
    [SerializeField] private List<Vector2Int> itemCoordinatesList;
    
    [SerializeField] private bool gravity = false;
    private List<PlacedGridObject> gravityItemsList = new List<PlacedGridObject>(); //using queue system to check what items to move
    private float gravityDelayTime = 0.05f;
    private float currentGravityDelayTime;

    [SerializeField] private Canvas gridCanvas;
    [SerializeField] private Camera gridCamera;

    #region GirdValue
    /*public class GridValue
    {
        private Grid<GridValue> grid;
        private int x;
        private int y;
        private PlacedGridObject placedGridObject;

        public GridValue(Grid<GridValue> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlacedGridObject(PlacedGridObject placedGridObject)
        {
            this.placedGridObject = placedGridObject;
            grid.TriggerGridValueChanged(x, y);
        }

        public PlacedGridObject GetPlacedGridObject() { return placedGridObject; }

        public void ClearPlacedGridObject()
        {
            placedGridObject = null;
            grid.TriggerGridValueChanged(x, y);
        }

        public bool CanBuild() { return placedGridObject == null; }
        public override string ToString() { return x + ", " + y + "\n" + placedGridObject; }
    }*/
    #endregion

    public OldGrid<GridCellValue> GetGrid() { return grid; }
    public int GetGridWidth() { return gridWidth; }
    public int GetGridHeight() { return gridHeight; }
    public float GetGridCellSize() { return (cellSize/100f); }
    public Transform GetStartingPosition() { return startingPosition; }
    public bool GetIsSearchable() { return isSearchable; }
    public List<Vector2Int> GetNullCells() { return nullCells; }
    public bool IsGravity() { return gravity; }
    public void AddToGravityItemsList(PlacedGridObject obj) { gravityItemsList.Add(obj); }
    public bool GravityItemsListContains(PlacedGridObject obj) 
    {
        if(obj != null)
        {
            //print(gravityItemsList.Contains(obj));
            return gravityItemsList.Contains(obj);
        }
        return true;
    }

    private void Awake()
    {
        if (!gridParent)
        {
            Debug.LogError(this + ": Grid Object's Parent is empty.");
        }

        if (itemList.Count > itemCoordinatesList.Count)
        {
            Debug.LogError(this + ": number of items does not match number of item coordinates.");
        }

        foreach(Vector2Int itemCoordinates in itemCoordinatesList)
        {
            if ((itemCoordinates.x >= gridWidth || itemCoordinates.x < 0) || (itemCoordinates.y >= gridHeight || itemCoordinates.y < 0))
            {
                Debug.LogError(this + ": " + itemCoordinates + " Item Coordinates List is out of bounds!");
            }
        }

        currentGravityDelayTime = gravityDelayTime;
    }

    // GridManager will wake up GridObjects
    public void AwakeScirpt()
    {
        FindCanvasAndCamera();
        CheckArrow();
        CreateGrid();
        GenerateTileImages();
        SpawnItemsInGrid();
    }

    private void Update()
    {
        GridGravity();
    }

    private void FindCanvasAndCamera()
    {
        gridCanvas = GameObject.FindGameObjectWithTag("GridCanvas").GetComponent<Canvas>();
        gridCamera = GameObject.FindGameObjectWithTag("GridCamera").GetComponent<Camera>();

        if (gridCanvas == null || gridCamera == null)
            Debug.LogError(this + ": missing reference to canvas or camera!");
    }

    private void CheckArrow()
    {
        if (arrow != null && arrow.activeSelf)
        {
            HideArrow();
        }
    }

    private void CreateGrid()
    {
        grid = new OldGrid<GridCellValue>(gridWidth, gridHeight, cellSize, gridParent, startingPosition.GetComponent<RectTransform>().anchoredPosition, (OldGrid<GridCellValue> g, int x, int y) => new GridCellValue(g, x, y));
        //print(startingPosition.gameObject.name + ": " + startingPosition.position);
        //print(startingPosition.gameObject.name + ": " + startingPosition.GetComponent<RectTransform>().anchoredPosition);
    }

    private void GenerateTileImages()
    {
        List<Vector2Int> entireGrid = new List<Vector2Int>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                entireGrid.Add(new Vector2Int(x, y));
            }
        }

        //print(canvas.scaleFactor);
        foreach (Vector2Int coordinates in entireGrid)
        {
            if (!nullCells.Contains(coordinates))
            {
                GameObject tiles = Instantiate(gridTile, grid.GetCanvasWorldPosition(gridCanvas, coordinates.x, coordinates.y, gridCamera), Quaternion.identity);
                //GameObject tiles = Instantiate(gridTile, grid.GetWorldPosition(coordinates.x, coordinates.y), Quaternion.identity);
                //tiles.transform.localScale *= cellSize/100f;
                //print(tiles.transform.localScale);
                //tiles.transform.localScale *= canvas.scaleFactor;
                tiles.transform.localScale *= gridCanvas.transform.localScale.x;
                //print(tiles.transform.localScale);
                tiles.transform.SetParent(gridParent);

                grid.GetGridCellValue(coordinates.x, coordinates.y).SetGridTile(tiles.GetComponent<GridTile>());
            }
        }
    }

    public bool IsMouseInThisGrid()
    {
        grid.GetXYPosition(Input.mousePosition, out int x, out int y);
        //print(x + ", " + y);
        
        if((x >= 0 && x < gridWidth) && (y >= 0 && y < gridHeight))
        {
            //print(gameObject.name + ": " + gridWidth + ", " + gridHeight);
            return true;
        }

        return false;
    }

    public void RevealArrow()
    {
        if (isSearchable)
            arrow.SetActive(true);
    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }

    public void MoveArrow(Vector3 tilePos)
    {
        if (isSearchable)
        {
            Vector3 tilePosition = new Vector3(tilePos.x, tilePos.y + arrowPadding, tilePos.z);
            //print(tilePosition);
            arrow.GetComponent<RectTransform>().position = tilePosition;
            //print(arrow.transform.position);
        }
    }

    public bool CheckCoordinatesOnGrid(ItemObject itemObject, Vector2Int coordinates, ItemObject.Dir direction)
    {
        int itemID = itemObject.GetComponent<PlacedGridObject>().GetItemID();
        List<Vector2Int> gridCoordinatesList = itemObject.GetCoordinateList(new Vector2Int(coordinates.x, coordinates.y), direction);

        foreach (Vector2Int coordinate in gridCoordinatesList)
        {
            // if coordinates are not on a grid then we can't place down an object
            if (grid.GetGridCellValue(coordinate.x, coordinate.y) == null)
            {
                //print("not in grid");
                //Debug.LogError(this + ": the given direction and coordinates are not in the grid.");
                return false;
            }

            // if coordinates already contains a DIFFERENT placed object, then we can't place down an object
            if (!grid.GetGridCellValue(coordinate.x, coordinate.y).IsPlacedGridObjectEmpty() && grid.GetGridCellValue(coordinate.x, coordinate.y).GetPlacedGridObjectItemID() != itemID)
            {
                //print("taken");
                //Debug.LogError(this + ": the given direction and coordinates are already occupied in the grid.");
                return false;
            }
        }
        
        return true;
    }

    private void SpawnItemsInGrid()
    {
        for(int i=0; i<itemList.Count; i++)
        {
            if (CheckCoordinatesOnGrid(itemList[i], itemCoordinatesList[i], itemDirectionsList[i]))
            {
                GridManager.Instance.SpawnItemInGrid(grid, itemList[i], itemCoordinatesList[i], itemDirectionsList[i], gridCanvas);
            }
        }
    }

    public void SpawnItemsInGrid(List<ItemObject> items, List<ItemObject.Dir> itemDirections, List<Vector2Int> itemCoordinates)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (CheckCoordinatesOnGrid(items[i], itemCoordinates[i], itemDirections[i]))
            {
                GridManager.Instance.SpawnItemInGrid(grid, items[i], itemCoordinates[i], itemDirections[i], gridCanvas);
            }
        }
    }

    private void GridGravity()
    {
        if (gravity && gravityItemsList.Count > 0)
        {
            //print(gravityItemsList.Count);
            if (currentGravityDelayTime > 0)
            {
                currentGravityDelayTime -= Time.deltaTime;
            }
            else
            {
                PlacedGridObject firstObjectInList = gravityItemsList[0];
                MoveItemDown(firstObjectInList);
                currentGravityDelayTime = gravityDelayTime;
            }
        }
    }

    private void MoveItemDown(PlacedGridObject objectToMove)
    {
        ItemObject item = objectToMove.GetComponent<ItemObject>();
        Vector2Int origin = objectToMove.GetGridPositionList()[0];
        Vector2Int destinationCoord = new Vector2Int(origin.x, origin.y-1);
        ItemObject.Dir direction = objectToMove.GetDir();

        if (CheckCoordinatesOnGrid(item, destinationCoord, direction))
        {
            //remove item from list
            gravityItemsList.Remove(gravityItemsList[0]);

            //Destroy Original item
            GridManager.GridCoordinate originalItem = new GridManager.GridCoordinate(this, origin.x, origin.y);
            GridManager.Instance.DestoryGridItem(originalItem);

            //Create same item at new coordinates
            GridManager.Instance.SpawnItemInGrid(grid, item, destinationCoord, direction, gridCanvas);
        }
    }

    private bool IsCellEmpty(int x, int y) => grid.GetGridCellValue(x, y).IsPlacedGridObjectEmpty();
}