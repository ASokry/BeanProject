using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSource : MonoBehaviour
{
    [Header("Debug")]
    public bool debugMode;

    [Header("Loot & Grid Stuff")]
    [SerializeField] private List<ItemObject> lootList;
    [SerializeField] private List<ItemObject.Dir> lootDirectionsList;
    [SerializeField] private List<Vector2Int> lootCoordinatesList;

    [SerializeField] private OldGridObject targetGridObject;

    private void Start()
    {
        if(debugMode == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

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

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AddLootToGridObject();
            print("Loot Got");
        }
    }
}
