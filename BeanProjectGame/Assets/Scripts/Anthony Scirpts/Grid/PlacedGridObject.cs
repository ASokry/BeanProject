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
        //placedGridObject.itemObject = itemObject;
        placedGridObject.itemObject = placedObject.GetComponent<ItemObject>();
        placedGridObject.origin = origin;
        placedGridObject.dir = dir;
        //placedGridObject.objectName = itemObject.GetItemName();
        placedGridObject.objectName = placedObject.GetComponent<ItemObject>().GetItemName();
        placedGridObject.itemID = placedGridObject.GetInstanceID();
        //placedGridObject.itemType = itemObject.GetObjType();
        placedGridObject.itemType = placedObject.GetComponent<ItemObject>().GetObjType();
        
        //print(placedGridObject);
        return placedGridObject;
    }

    [SerializeField] private ItemObject itemObject;
    private Vector2Int origin;
    private ItemObject.Dir dir;
    [SerializeField] private string objectName;
    private int itemID;
    private ItemObject.itemTyp itemType;
    private Transform parentGrid;

    public Transform GetParentGrid() { return parentGrid; }
    public string GetObjectName() { return objectName; }
    public int GetItemID() { return itemID; }
    public ItemObject GetItemObject() { return itemObject; }
    public ItemObject.Dir GetDir() { return dir; }
    public ItemObject.itemTyp GetItemType() { return itemType; }

    public List<Vector2Int> GetGridPositionList()
    {
        return itemObject.GetCoordinateList(origin, dir);
    }

    public void DestroySelf() { Destroy(gameObject); }
}
