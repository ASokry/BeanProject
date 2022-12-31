using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetris : MonoBehaviour {
    public event EventHandler<PlacedObject> OnObjectPlaced;

    [SerializeField, Range(1, 10)] private int gridWidthMax = 10;
    [SerializeField, Range(1, 10)] private int gridHeightMax = 10;
    [SerializeField, Range(1, 50)] private float cellSize = 50f;

    [SerializeField] private bool nullTiles = false;
    [SerializeField, Range(0, 10)] private int gridWidthMin = 0;
    [SerializeField, Range(0, 10)] private int gridHeightMin = 0;
    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;
    [SerializeField] private List<Vector2Int> customNullTiles = new List<Vector2Int>();
    [SerializeField] private bool upgradeTiles = false;

    public int GetWidth() { return gridWidthMax; }
    public int GetHeight() { return gridHeightMax; }

    private Grid<GridObject> grid;
    private RectTransform itemContainer;
    [SerializeField] private bool isActiveGrid = true;
    public void SetActiveGrid(bool b) { isActiveGrid = b; }
    public bool GetActiveGrid() { return isActiveGrid; }

    [SerializeField] private bool inventoryTetrisGravity = false;
    public bool Gravity() { return inventoryTetrisGravity; }
    private void Awake() {
        grid = new Grid<GridObject>(gridWidthMax, gridHeightMax, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        if (nullTiles && !inventoryTetrisBackground) Debug.LogError("inventoryTetrisBackground Reference is missing");

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);
    }

    private void Start()
    {
        CalculateNullRowsCols();
        CalculateCustomNullTiles();
    }

    private void Update()
    {
        ExpandGrid();
    }

    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

        public bool HasPlacedObject() {
            return placedObject != null;
        }
    }

    public Grid<GridObject> GetGrid() {
        return grid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return grid.IsValidGridPosition(gridPosition);
    }

    private void CalculateCustomNullTiles()
    {
        if (nullTiles && customNullTiles.Count > 0)
        {
            SetNullTiles(customNullTiles);
        }
    }

    private void CalculateNullRowsCols()
    {
        if (nullTiles == true && (gridWidthMin > 0 && gridHeightMin > 0))
        {
            int xRemainder = gridWidthMax - gridWidthMin;
            int yRemainder = gridHeightMax - gridHeightMin;
            int xRightNums = xRemainder / 2;
            int xTopNums = yRemainder / 2;
            //print(colRemainder + ", " + colRightNums + ": " + rowRemainder + ", " + rowTopNums);
            List<int> xNums = new List<int>();
            List<int> yNums = new List<int>();

            int count = 0;
            while (xNums.Count != xRemainder || yNums.Count != yRemainder)
            {
                if (xNums.Count != xRemainder)
                {
                    int num = xNums.Count > xRightNums ? (count + gridWidthMin) : count;
                    xNums.Add(num);
                }

                if (yNums.Count != yRemainder)
                {
                    int num = yNums.Count < xTopNums ? count : (count + gridHeightMin);
                    yNums.Add(num);
                }

                count++;
            }
            //foreach (int i in colNums) print(i);

            List<Vector2Int> tiles = new List<Vector2Int>();
            count = 0;
            while (count < gridWidthMax || count < gridHeightMax)
            {
                if (count < gridWidthMax)
                {
                    foreach (int x in xNums)
                    {
                        //print(col);
                        Vector2Int coordinate = new Vector2Int(x, count);
                        if (!tiles.Contains(coordinate)) tiles.Add(coordinate);
                    }
                }

                if (count < gridHeightMax)
                {
                    foreach (int y in yNums)
                    {
                        Vector2Int coordinate = new Vector2Int(count, y); //print(coordinate);
                        if (!tiles.Contains(coordinate)) tiles.Add(coordinate);
                    }
                }
                count++;
            }
            //foreach (Vector2Int i in tiles) print(i);
            SetNullTiles(tiles);
        }
    }

    private void SetNullTiles(List<Vector2Int> coordinates)
    {
        if (inventoryTetrisBackground)
        {
            foreach (Vector2Int coord in coordinates)
            {
                //print(coord);
                InventoryTileSystem.Instance.SetTile(inventoryTetrisBackground, coord, InventoryTileSystem.TileType.Null);
            }
        }
    }

    private void ExpandGrid()
    {
        if (upgradeTiles && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject() && Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = GetGridPosition(anchoredPosition);
            inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(mouseGridPosition, out InventoryTile tile);
            if (tile.IsNull())
            {
                //print("Expand");
                InventoryTileSystem.Instance.SetTile(inventoryTetrisBackground, mouseGridPosition, InventoryTileSystem.TileType.Default);
            }
        }
    }

    public bool CheckBuildItemPositions(List<Vector2Int> gridPositionList, PlacedObject placedObject)
    {
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            //print(gridPosition);
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                return false;
            }

            if (inventoryTetrisBackground)
            {
                inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(gridPosition, out InventoryTile tile);
                if (tile.IsNull()) return false;
            }

            GridObject gridObject = grid.GetGridObject(gridPosition.x, gridPosition.y);
            if (!gridObject.CanBuild() && gridObject.GetPlacedObject().GetID() != placedObject.GetID())
            {
                //print("there's a different item at" + ", " + gridPosition);
                return false;
            }
        }
        return true;
    }

    private bool CheckBuildItemPositions(List<Vector2Int> gridPositionList)
    {
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            bool isValidPosition = grid.IsValidGridPosition(gridPosition);
            if (!isValidPosition)
            {
                // Not valid
                return false;
            }

            if (inventoryTetrisBackground)
            {
                inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(gridPosition, out InventoryTile tile);
                if (tile.IsNull()) return false;
            }
            
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) return false;
        }
        return true;
    }

    public bool TryPlaceItem(ItemTetrisSO itemTetrisSO, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir) {
        // Test Can Build
        List<Vector2Int> gridPositionList = itemTetrisSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = CheckBuildItemPositions(gridPositionList);

        if (canPlace) {
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canPlace = false;
                    break;
                }
            }
        }

        if (canPlace) {
            Vector2Int rotationOffset = itemTetrisSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
            
            PlacedObject placedObject = PlacedObject.CreateCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir, itemTetrisSO);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -itemTetrisSO.GetRotationAngle(dir));

            //Set InventoryTetris reference in applicable scripts
            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);
            placedObject.GetComponent<InventoryGravity>().Setup(this);
            //
            

            foreach (Vector2Int gridPosition in gridPositionList) {
                //print(gridPosition);
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);

            // Object Placed!
            return true;
        } else {
            // Object CANNOT be placed!
            return false;
        }
    }

    public void TryMoveItem(PlacedObject placedObject, Vector2Int placedObjectOrigin, PlacedObjectTypeSO.Dir dir)
    {
        PlacedObjectTypeSO placedObjectTypeSO = placedObject.GetPlacedObjectTypeSO();
        List<Vector2Int> gridPositionList = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(placedObjectOrigin, dir);
        bool canPlace = CheckBuildItemPositions(gridPositionList, placedObject);

        if (canPlace)
        {
            ClearItemAt(placedObject.GetGridPosition());
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            placedObject.MoveCanvas(itemContainer, placedObjectWorldPosition, placedObjectOrigin, dir);
            placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

            //Set InventoryTetris reference in applicable scripts
            placedObject.GetComponent<InventoryTetrisDragDrop>().Setup(this);
            placedObject.GetComponent<InventoryGravity>().Setup(this);
            //


            foreach (Vector2Int gridPosition in gridPositionList)
            {
                //print(gridPosition);
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            OnObjectPlaced?.Invoke(this, placedObject);

            // Object Moved!
            //print("Moved");
        }
        else
        {
            // Object CANNOT be Moved!
            //print("Cannot Move");
        }
    }

    public void RemoveItemAt(Vector2Int removeGridPosition) {
        PlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();

        if (placedObject != null) {
            // Demolish
            placedObject.DestroySelf();

            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    public void ClearItemAt(Vector2Int removeGridPosition)
    {
        PlacedObject placedObject = grid.GetGridObject(removeGridPosition.x, removeGridPosition.y).GetPlacedObject();
        if (placedObject != null)
        {
            //Clear PlacedObject data from gridObject
            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }
        }
    }

    public RectTransform GetItemContainer() {
        return itemContainer;
    }



    [Serializable]
    public struct AddItemTetris {
        public string itemTetrisSOName;
        public Vector2Int gridPosition;
        public PlacedObjectTypeSO.Dir dir;
    }

    [Serializable]
    public struct ListAddItemTetris {
        public List<AddItemTetris> addItemTetrisList;
    }

    public string Save() {
        List<PlacedObject> placedObjectList = new List<PlacedObject>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                if (grid.GetGridObject(x, y).HasPlacedObject()) {
                    placedObjectList.Remove(grid.GetGridObject(x, y).GetPlacedObject());
                    placedObjectList.Add(grid.GetGridObject(x, y).GetPlacedObject());
                }
            }
        }

        List<AddItemTetris> addItemTetrisList = new List<AddItemTetris>();
        foreach (PlacedObject placedObject in placedObjectList) {
            addItemTetrisList.Add(new AddItemTetris {
                dir = placedObject.GetDir(),
                gridPosition = placedObject.GetGridPosition(),
                itemTetrisSOName = (placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO).name,
            });
        }

        return JsonUtility.ToJson(new ListAddItemTetris { addItemTetrisList = addItemTetrisList });
    }

    public void Load(string loadString) {
        ListAddItemTetris listAddItemTetris = JsonUtility.FromJson<ListAddItemTetris>(loadString);

        foreach (AddItemTetris addItemTetris in listAddItemTetris.addItemTetrisList) {
            TryPlaceItem(InventoryTetrisAssets.Instance.GetItemTetrisSOFromName(addItemTetris.itemTetrisSOName), addItemTetris.gridPosition, addItemTetris.dir);
        }
    }

}
