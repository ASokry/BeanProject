using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedGridObject : MonoBehaviour
{
    private ItemObject itemObject;
    private Vector2Int origin;
    private ItemObject.Dir dir;
    private string objectName;

    public static PlacedGridObject Create(Vector3 worldPosition, Vector2Int origin, ItemObject.Dir dir, ItemObject itemObject)
    {
        Transform placedObjectTransform = Instantiate(itemObject.GetPrefab(), worldPosition, Quaternion.Euler(0, 0, itemObject.GetRotationAngle(dir)));

        PlacedGridObject placedGridObject = placedObjectTransform.GetComponent<PlacedGridObject>();

        placedGridObject.itemObject = itemObject;
        placedGridObject.origin = origin;
        placedGridObject.dir = dir;
        placedGridObject.objectName = itemObject.nameString;

        return placedGridObject;
    }

    public string GetObjectName() { return objectName; }
    public ItemObject.Dir GetDir() { return dir; }

    public List<Vector2Int> GetGridPositionList()
    {
        return itemObject.GetCoordinateList(origin, dir);
    }

    public void DestroySelf() { Destroy(gameObject); }
}
