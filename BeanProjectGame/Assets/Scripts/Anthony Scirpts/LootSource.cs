using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSource : MonoBehaviour
{
    [SerializeField] private List<ItemObject> lootList;
    [SerializeField] private List<ItemObject.Dir> lootDirectionsList;
    [SerializeField] private List<Vector2Int> lootCoordinatesList;

    [SerializeField] private GridObject targetGridObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("LOOT!");
            AddLootToGridObject();
        }
    }

    public void AddLootToGridObject()
    {
        if (lootList.Count == lootDirectionsList.Count && lootList.Count == lootCoordinatesList.Count)
        {
            targetGridObject.SpawnItemsInGrid(lootList, lootDirectionsList, lootCoordinatesList);
        }
    }
}
