using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySearchSystem : MonoBehaviour
{
    public static InventorySearchSystem Instance { get; private set; }

    [SerializeField] private List<InventorySearch> gridSearchList;
    private bool canContinue = false;
    public void CanContinue(bool b) { canContinue = b; }

    public InventoryItem foundItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && foundItem == null)
        {
            StartCoroutine(SearchThroughGrids("H-Ammo"));
        }
    }

    public void AddToSearchList(InventorySearch gridSearch)
    {
        gridSearchList.Add(gridSearch);
    }

    public void SetFoundItem(InventoryItem inventoryItem)
    {
        foundItem = inventoryItem;
    }

    public IEnumerator SearchThroughGrids(string target)
    {
        int index = gridSearchList.Count-1;
        while (index >=0)
        {
            InventorySearch gridToSearch = gridSearchList[index];
            gridToSearch.StartGridTraversal(target);
            yield return new WaitUntil(() => canContinue == true);

            //currently set to always be null, see InventorySearch
            if (foundItem != null)
            {
                yield break;
            }

            index--;
            canContinue = false;
        }
        print("done");
    }

    //Must use following method after we're done using foundItem in Austin's script 
    public void ResetSearchSystem()
    {
        foundItem = null;
    }
}
