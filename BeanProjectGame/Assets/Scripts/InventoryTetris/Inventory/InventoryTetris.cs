using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetris : MonoBehaviour {
    public event EventHandler<PlacedObject> OnObjectPlaced;

    [SerializeField, Range(1, 10)] private int gridWidthMax = 10;
    [SerializeField, Range(1, 10)] private int gridHeightMax = 10;
    [SerializeField, Range(1, 50)] private float cellSize = 50f;

    [SerializeField] private bool startWithNullTiles = false;
    [SerializeField, Range(0, 10)] private int gridWidthMin = 0;
    [SerializeField, Range(0, 10)] private int gridHeightMin = 0;
    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;
    [SerializeField] private List<Vector2Int> customNullTiles = new List<Vector2Int>();

    [SerializeField] private bool allowUpgradeTiles = false;
    private List<Vector2Int> allUpgradeableTiles = new List<Vector2Int>();
    private List<Vector2Int> tilesToUpdgrade = new List<Vector2Int>();

    public int GetWidthMax() { return gridWidthMax; }
    public int GetHeightMax() { return gridHeightMax; }
    public InventoryTetrisBackground GetInventoryTetrisBackground() { return inventoryTetrisBackground; }

    private Grid<GridObject> grid;
    private RectTransform itemContainer;
    [SerializeField] private bool isActiveGrid = true;
    public void SetActiveGrid(bool b) { isActiveGrid = b; }
    public bool GetActiveGrid() { return isActiveGrid; }

    [SerializeField] private bool inventoryTetrisGravity = false;
    public bool Gravity() { return inventoryTetrisGravity; }
    private void Awake() {
        grid = new Grid<GridObject>(gridWidthMax, gridHeightMax, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        if (startWithNullTiles && !inventoryTetrisBackground) Debug.LogError("inventoryTetrisBackground Reference is missing");

        itemContainer = transform.Find("ItemContainer").GetComponent<RectTransform>();

        transform.Find("BackgroundTempVisual").gameObject.SetActive(false);
    }

    private void Start()
    {
        InventoryGridManager.Instance.AddToManager(this);
    }

    public void SetupTiles()
    {
        CalculateNullRowsCols();
        CalculateCustomNullTiles();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.U) && InventoryGridManager.Instance.GetCurrentState() != InventoryGridManager.InventoryState.Upgrading)
        {
            InventoryGridManager.Instance.SetCurrentState(InventoryGridManager.InventoryState.Upgrading);
            InventoryGridManager.Instance.SetStartingUpgradePoints(3);
            ShowUpgradeableTiles();
        }
        ClickToExpandGrid();*/
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
        if (startWithNullTiles && customNullTiles.Count > 0)
        {
            SetNullTiles(customNullTiles);
        }
    }

    private void CalculateNullRowsCols()
    {
        if (startWithNullTiles == true && (gridWidthMin > 0 && gridHeightMin > 0))
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
                InventoryTileSystem.Instance.SetTileSprite(inventoryTetrisBackground, coord, InventoryTileSystem.TileType.Null);
            }
        }
    }

    private bool CanUpgrade()
    {
        return InventoryGridManager.Instance.GetCurrentState() == InventoryGridManager.InventoryState.Upgrading && InventoryGridManager.Instance.CheckUpgradePoints() > 0;
    }

    private void ShowUpgradeableTiles()
    {
        //print(allowUpgradeTiles && CanUpgrade());
        if (allowUpgradeTiles && CanUpgrade())
        {
            Dictionary<Vector2Int, InventoryTile> tiles = inventoryTetrisBackground.GetInventoryTileDictionary();
            foreach (KeyValuePair<Vector2Int, InventoryTile> valuePair in tiles)
            {
                if (valuePair.Value.IsUpgradeable() || !valuePair.Value.IsNull()) continue;
                Vector2Int leftTileCoord = new Vector2Int(valuePair.Key.x - 1, valuePair.Key.y);
                Vector2Int rightTileCoord = new Vector2Int(valuePair.Key.x + 1, valuePair.Key.y);
                Vector2Int belowTileCoord = new Vector2Int(valuePair.Key.x, valuePair.Key.y - 1);
                Vector2Int aboveTileCoord = new Vector2Int(valuePair.Key.x, valuePair.Key.y + 1);
                Vector2Int[] adjacentCoordinates = { leftTileCoord, rightTileCoord, belowTileCoord, aboveTileCoord };
                foreach (Vector2Int coordinate in adjacentCoordinates)
                {
                    bool isTileNull = InventoryTileSystem.Instance.IsTileNull(inventoryTetrisBackground, coordinate);
                    bool isTileUpgradeable = InventoryTileSystem.Instance.IsTileUpgradeable(inventoryTetrisBackground, coordinate);
                    //if(allowUpgradeTiles) print(!isTileNull && !isTileUpgradeable);
                    if (!isTileNull && !isTileUpgradeable)
                    {
                        //print(valuePair.Key);
                        //valuePair.Value.SetUpgradeable(true);
                        //Set tile upgradeable status to true (tile can now be upgraded when selected)
                        InventoryTileSystem.Instance.SetTileUpgradeable(inventoryTetrisBackground, valuePair.Key, true);
                        InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, valuePair.Key, InventoryTileSystem.TileOverlayType.Upgradeable);
                        allUpgradeableTiles.Add(valuePair.Key);
                        break;
                    }
                }
            }
        }
    }

    private void ClickToExpandGrid()
    {
        if (CanUpgrade() && allowUpgradeTiles && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject() && Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = GetGridPosition(anchoredPosition);
            bool isValidPosition = grid.IsValidGridPosition(mouseGridPosition);
            bool isTileNull = InventoryTileSystem.Instance.IsTileNull(inventoryTetrisBackground, mouseGridPosition);
            bool isTileUpgradeable = InventoryTileSystem.Instance.IsTileUpgradeable(inventoryTetrisBackground, mouseGridPosition);
            if (isValidPosition && isTileNull && isTileUpgradeable)
            {
                //print(mouseGridPosition);
                AddToUpgradeList(mouseGridPosition);
                //UpgradeTile(mouseGridPosition);
            }
        }
    }

    private void AddToUpgradeList(Vector2Int coordinate)
    {
        tilesToUpdgrade.Add(coordinate);
        InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileOverlayType.Default);
        InventoryTileSystem.Instance.SetTileSprite(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileType.Upgrade);
        InventoryGridManager.Instance.SetUpgradePoints(InventoryGridManager.Instance.CheckUpgradePoints() - 1);
    }

    public void UpgradeTiles()
    {
        if (allowUpgradeTiles)
        {
            foreach (Vector2Int coordinate in tilesToUpdgrade)
            {
                UpgradeTile(coordinate);
                allUpgradeableTiles.Remove(coordinate);
            }
            tilesToUpdgrade.Clear();
            ClearAllUpgradeableTiles();
            InventoryGravitySystem.Instance.TriggerGravitySystem();
        }
    }

    private void UpgradeTile(Vector2Int coordinate)
    {
        InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileOverlayType.Default);
        InventoryTileSystem.Instance.SetTileSprite(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileType.Default);
        //InventoryGridManager.Instance.SetUpgradePoints(InventoryGridManager.Instance.CheckUpgradePoints() - 1);
        //InventoryGravitySystem.Instance.TriggerGravitySystem();
    }

    private void ClearAllUpgradeableTiles()
    {
        foreach (Vector2Int coordinate in allUpgradeableTiles)
        {
            InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileOverlayType.Default);
            InventoryTileSystem.Instance.SetTileSprite(inventoryTetrisBackground, coordinate, InventoryTileSystem.TileType.Null);
            InventoryTileSystem.Instance.SetTileUpgradeable(inventoryTetrisBackground, coordinate, false);
        }
        allUpgradeableTiles.Clear();
    }

    public void ResetUpgradeables()
    {
        if (allowUpgradeTiles)
        {
            ClearAllUpgradeableTiles();
            tilesToUpdgrade.Clear();
            ShowUpgradeableTiles();
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

            if (inventoryTetrisBackground && InventoryTileSystem.Instance.IsTileNull(inventoryTetrisBackground, gridPosition))
            {
                return false;
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
            //ClearItemAt(placedObject.GetGridPosition());
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
