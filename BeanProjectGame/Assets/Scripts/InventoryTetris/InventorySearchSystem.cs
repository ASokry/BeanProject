using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySearchSystem : MonoBehaviour
{
    public static InventorySearchSystem Instance { get; private set; }

    [SerializeField] private List<InventorySearch> gridSearchList;
    private bool canContinue = false;
    public void CanContinue(bool b) { canContinue = b; }

    public ItemObject foundItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SearchThroughGrids("H-Ammo"));
        }
    }

    public void AddToSearchList(InventorySearch gridSearch)
    {
        gridSearchList.Add(gridSearch);
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
}
