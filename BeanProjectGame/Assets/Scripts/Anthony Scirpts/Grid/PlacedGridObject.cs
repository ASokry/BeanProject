using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedGridObject : MonoBehaviour
{
    public static PlacedGridObject Create(Vector3 worldPosition, Vector2Int origin, ItemObject.Dir dir, ItemObject itemObject, Transform parentGrid, float canvasScale)
    {
        GameObject placedObject = Instantiate(itemObject.GetPrefab(), worldPosition, Quaternion.Euler(0, 0, itemObject.GetRotationAngle(dir)));
        placedObject.transform.localScale *= canvasScale;
        //print(placedObject.GetComponent<RectTransform>().localScale);
        placedObject.GetComponent<ItemObject>().SetCurrentDirection(dir);
        placedObject.GetComponent<Transform>().SetParent(parentGrid);
        placedObject.name = itemObject.GetItemName();

        PlacedGridObject placedGridObject = placedObject.GetComponent<PlacedGridObject>();

        placedGridObject.parentGrid = parentGrid;
        placedGridObject.isGravity = parentGrid.GetComponent<GridObject>().IsGravity();
        //placedGridObject.itemObject = itemObject;
        placedGridObject.itemObject = placedObject.GetComponent<ItemObject>();
        placedGridObject.origin = origin;
        placedGridObject.dir = dir;
        //placedGridObject.objectName = itemObject.GetItemName();
        placedGridObject.objectName = placedObject.GetComponent<ItemObject>().GetItemName();
        placedGridObject.itemID = placedGridObject.GetInstanceID();
        //placedGridObject.itemType = itemObject.GetObjType();
        placedGridObject.itemType = placedObject.GetComponent<ItemObject>().GetItemType();

        GridManager.Instance.OnItemSpawnedInGrid += placedGridObject.StartItemGravity;
        //print(placedGridObject);
        return placedGridObject;
    }

    [SerializeField] private ItemObject itemObject;
    private Vector2Int origin;
    private ItemObject.Dir dir;
    [SerializeField] private string objectName;
    private int itemID;
    private ItemObject.ItemType itemType;
    private Transform parentGrid;
    private bool isGravity;

    public Transform GetParentGrid() { return parentGrid; }
    public string GetObjectName() { return objectName; }
    public int GetItemID() { return itemID; }
    public ItemObject GetItemObject() { return itemObject; }
    public ItemObject.Dir GetDir() { return dir; }
    public ItemObject.ItemType GetItemType() { return itemType; }
    public bool GetIsGravity() { return isGravity; }

    public List<Vector2Int> GetGridPositionList()
    {
        return itemObject.GetCoordinateList(origin, dir);
    }

    public void DestroySelf() { Destroy(gameObject); }

    public void StartItemGravity()
    {
        GridObject gridObject = parentGrid.GetComponent<GridObject>();
        Vector2Int cellBelowCoordinates = new Vector2Int(origin.x, origin.y-1);
        //print(gridObject.CheckCoordinatesOnGrid(itemObject, cellBelowCoordinates, dir) + " " + this.gameObject.GetInstanceID());
        //print(gameObject.GetComponent<PlacedGridObject>() + " " + gameObject.GetInstanceID());
        //print(gridObject.GravityItemsListContains(gameObject.GetComponent<PlacedGridObject>()));
        if (isGravity && gridObject.CheckCoordinatesOnGrid(itemObject, cellBelowCoordinates, dir) && !gridObject.GravityItemsListContains(this))
        {
            //print("added");
            gridObject.AddToGravityItemsList(this);
        }
    }

    private void OnDestroy()
    {
        //always remember to unsubscribe from GridManager event
        GridManager.Instance.OnItemSpawnedInGrid -= StartItemGravity;
    }
}
