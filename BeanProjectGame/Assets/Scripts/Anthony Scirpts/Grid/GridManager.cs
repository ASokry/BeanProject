using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helps with moving items from gird to grid
public class GridManager : MonoBehaviour
{
    private List<GridObject> gridObjectList;

    private ItemObject item;
    private ItemObject.Dir dir = ItemObject.Dir.Down;

    private Transform ghostObject;
    private bool ghostFollow = false;

    private bool canPlace = false;
    private bool inventoryClear = false;
}
