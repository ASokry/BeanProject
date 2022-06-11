using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedGridObject : MonoBehaviour
{
    private ItemObject itemObject;
    private Vector2Int origin;
    private ItemObject.Dir dir;
    private string objectName;
    private ItemObject.itemTyp itemType;
    private Transform parentGrid;

    public static PlacedGridObject Create(Vector3 worldPosition, Vector2Int origin, ItemObject.Dir dir, ItemObject itemObject, Transform parentGrid)
    {
        Transform placedObjectTransform = Instantiate(itemObject.GetPrefab(), worldPosition, Quaternion.Euler(0, 0, itemObject.GetRotationAngle(dir)));
        placedObjectTransform.SetParent(parentGrid);

        PlacedGridObject placedGridObject = placedObjectTransform.GetComponent<PlacedGridObject>();

        placedGridObject.parentGrid = parentGrid;
        placedGridObject.itemObject = itemObject;
        placedGridObject.origin = origin;
        placedGridObject.dir = dir;
        placedGridObject.objectName = itemObject.nameString;
        placedGridObject.itemType = itemObject.GetObjType();

        return placedGridObject;
    }

    public Transform GetParentGrid() { return parentGrid; }
    public string GetObjectName() { return objectName; }
    public ItemObject.Dir GetDir() { return dir; }
    public ItemObject.itemTyp GetItemType() { return itemType; }

    public List<Vector2Int> GetGridPositionList()
    {
        return itemObject.GetCoordinateList(origin, dir);
    }

    public void DestroySelf() { Destroy(gameObject); }
}
