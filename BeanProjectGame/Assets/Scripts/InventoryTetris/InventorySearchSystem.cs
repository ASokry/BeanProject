using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySearchSystem : MonoBehaviour
{
    public static InventorySearchSystem Instance { get; private set; }

    [SerializeField] private List<InventorySearch> gridSearchList;
    private bool isSearching = false;
    private bool canContinue = false;
    public void CanContinue(bool b) { canContinue = b; }

    public InventoryItem foundItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        StartGridSearch("H-Ammo");
    }

    public void AddToSearchList(InventorySearch gridSearch)
    {
        gridSearchList.Add(gridSearch);
    }

    public void SetFoundItem(InventoryItem inventoryItem)
    {
        foundItem = inventoryItem;
    }

    private bool CanSearch()
    {
        return InventoryGridManager.Instance.GetCurrentState() == InventoryGridManager.InventoryState.Locked && foundItem == null && !isSearching;
    }

    public void StartGridSearch(string name)
    {
        if (Input.GetKeyDown(KeyCode.S) && CanSearch())
        {
            StartCoroutine(SearchThroughGrids(name));
        }
    }

    private IEnumerator SearchThroughGrids(string target)
    {
        isSearching = true;
        int index = gridSearchList.Count-1;
        while (index >=0)
        {
            InventorySearch gridToSearch = gridSearchList[index];
            gridToSearch.StartGridTraversal(target);
            yield return new WaitUntil(() => canContinue == true);

            //currently set to always be null, see InventorySearch
            if (foundItem != null)
            {
                isSearching = false;
                yield break;
            }

            index--;
            canContinue = false;
        }
        //print("Search is done");
        isSearching = false;
    }

    //Must use this method after we're done using foundItem in Austin's script 
    public void ResetSearchSystem()
    {
        foundItem = null;
    }
}
