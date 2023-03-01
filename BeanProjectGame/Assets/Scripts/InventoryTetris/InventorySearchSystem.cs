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
    private InventoryTetris foundItemInventoryTetris;
    private Vector2Int foundItemCoordinate;

    public bool SearchState() { return isSearching; }
    private void Awake()
    {
        Instance = this;
    }

    public void AddToSearchList(InventorySearch gridSearch)
    {
        gridSearchList.Add(gridSearch);
    }

    public void SetFoundItem(InventoryItem inventoryItem, InventoryTetris inventoryTetris, Vector2Int coordinate)
    {
        foundItem = inventoryItem;
        foundItemInventoryTetris = inventoryTetris;
        foundItemCoordinate = coordinate;
        //print(foundItemCoordinate);
    }

    private bool CanSearch()
    {
        //return InventoryGridManager.Instance.GetCurrentState() == InventoryGridManager.InventoryState.Locked && foundItem == null && !isSearching;
        return foundItem == null && !isSearching;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGridSearch("H-Ammo");
        }
    }
    public void StartGridSearch(string name)
    {
        if (CanSearch()) StartCoroutine(SearchThroughGrids(name));
    }

    public void StartGridSearch(PlacedObject placedObject)
    {
        if (CanSearch()) StartCoroutine(SearchThroughGrids(placedObject));
    }

    //Search for string target
    private IEnumerator SearchThroughGrids(string strTarget)
    {
        isSearching = true;
        int index = gridSearchList.Count-1;
        while (index >=0)
        {
            InventorySearch gridToSearch = gridSearchList[index];
            gridToSearch.StartGridTraversal(strTarget);
            yield return new WaitUntil(() => canContinue == true);

            if (foundItem != null)
            {
                isSearching = false;
                canContinue = false;
                yield break;
            }

            index--;
            canContinue = false;
        }
        //print("Search is done");
        isSearching = false;
    }

    //Search for PlacedObject
    private IEnumerator SearchThroughGrids(PlacedObject placedObjTarget)
    {
        isSearching = true;
        int index = gridSearchList.Count - 1;
        while (index >= 0)
        {
            InventorySearch gridToSearch = gridSearchList[index];
            gridToSearch.StartGridTraversal(placedObjTarget);
            yield return new WaitUntil(() => canContinue == true);

            if (foundItem != null)
            {
                isSearching = false;
                canContinue = false;
                yield break;
            }

            index--;
            canContinue = false;
        }
        
        isSearching = false;
    }

    //Must use this method after we're done using foundItem in Austin's script 
    public void ResetSearchSystem()
    {
        foundItem = null;
        foundItemInventoryTetris = null;
        foundItemCoordinate = Vector2Int.zero;
    }

    public void DestroyFoundItem()
    {
        foundItem.DestroyThisItemOnGrid(foundItemInventoryTetris, foundItemCoordinate);
    }
}
