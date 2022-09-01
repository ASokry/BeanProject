using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract Class for sciprts/Objects that uses a Grid
public class GridObject : MonoBehaviour
{
    private Grid<GridCellValue> grid;
    [SerializeField] private Transform gridParent;

    public enum GridType { BackpackMain, BackpackOther, Inventory, QuickSlot };

    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 100f; //Cellsize was intially 100, and equal to width/height of recttransform on Tile Image
    [SerializeField] private Transform startingPosition;
    [SerializeField] private GameObject gridTile;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowPadding;

    [SerializeField] private List<Vector2Int> nullCells;

    [SerializeField] private List<ItemObject> itemList;
    [SerializeField] private bool gravity = false;

    public Canvas canvas;
    public Camera theCamera;

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

    public Grid<GridCellValue> GetGrid() { return grid; }
    public int GetGridWidth() { return gridWidth; }
    public int GetGridHeight() { return gridHeight; }
    public float GetGridCellSize() { return (cellSize/100f); }
    public Transform GetStartingPosition() { return startingPosition; }
    public List<Vector2Int> GetNullCells() { return nullCells; }

    private void Awake()
    {
        if (!gridParent)
        {
            Debug.LogError(this + ": Grid Object's Parent is empty.");
        }
    }

    // GridManager will wake up GridObjects
    public void AwakeScirpt()
    {
        CheckArrow();
        CreateGrid();
        GenerateTileImages();
        //SpawnItemsInGrid();
        SpawnItemsInGrid();
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
        grid = new Grid<GridCellValue>(gridWidth, gridHeight, cellSize, gridParent, startingPosition.position, (Grid<GridCellValue> g, int x, int y) => new GridCellValue(g, x, y));
        //print(startingPosition.gameObject.name + ": " + startingPosition.position);
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

        foreach (Vector2Int coordinates in entireGrid)
        {
            if (!nullCells.Contains(coordinates))
            {

                //GameObject tiles = Instantiate(gridTile, grid.GetCanvasWorldPosition(canvas, coordinates.x, coordinates.y, theCamera), Quaternion.identity);
                GameObject tiles = Instantiate(gridTile, grid.GetWorldPosition(coordinates.x, coordinates.y), Quaternion.identity);
                tiles.transform.localScale *= (cellSize/100f);
                //print(tiles.transform.position);
                tiles.transform.SetParent(gridParent);
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
        arrow.SetActive(true);
    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }

    public void MoveArrow(Vector3 tilePos)
    {
        Vector2 tilePosition = new Vector2(tilePos.x, tilePos.y + arrowPadding);
        arrow.transform.position = tilePosition;
    }

    private void SpawnItemsInGrid()
    {
        int x = 0;
        foreach (ItemObject item in itemList)
        {
            GridManager.Instance.SpawnItemInGrid(grid, item, new Vector2Int(x,1), ItemObject.Dir.Down);
            x++;
        }
    }

    private void GridGravity()
    {
        if (gravity)
        {
            for (int row = 1; row < gridWidth; row++)
            {
                for (int col = 0; col < gridHeight; col++)
                {
                    if (!IsCellEmpty(col, row))
                    {
                        
                    }
                }
            }
        }
    }

    private bool IsCellEmpty(int x, int y) => grid.GetGridCellValue(x, y).IsPlacedGridObjectEmpty();
}
