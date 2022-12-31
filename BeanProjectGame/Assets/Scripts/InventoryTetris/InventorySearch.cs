using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySearch : MonoBehaviour
{
    private bool searchState = false;
    [SerializeField] private bool inCombat = true;

    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private bool useArrow = false;
    [SerializeField] private InventoryArrow inventoryArrow;

    [SerializeField] private float searchDelay = 1f;

    [SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;

    public void StartGridTraversal(string target)
    {
        if ((target != null || target != "") && inCombat)
        {
            //print("start grid traversal");
            searchState = true;
            StartCoroutine(GridTraversal(target));
        }
        else
        {
            Debug.LogError("Player is not in combat or Search Target is invalid or empty.");
        }
    }

    private IEnumerator GridTraversal(string target)
    {
        // Traverse through entire grid, starting at the top row
        int row = GetTopMostRow(inventoryTetris);
        InventoryTileSystem.TileOverlayType searchType = InventoryTileSystem.TileOverlayType.Search;
        InventoryTileSystem.TileOverlayType defaultType = InventoryTileSystem.TileOverlayType.Default;

        while (row >= 0 && searchState)
        {
            //Reveal and move the arrow along y axis of grid on left hand side
            if (useArrow)
            {
                inventoryArrow.SetMax(inventoryTetris.GetWidth());
                inventoryArrow.ResetFill();
                inventoryArrow.MoveArrow(0, row);
                inventoryArrow.Reveal();
            }

            // search through each column of current row
            for (int col = 0; col < inventoryTetris.GetWidth(); col++)
            {
                Vector2Int coordinate = new Vector2Int(col, row);
                InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, searchType);
                if (useArrow) { inventoryArrow.Fill(); }
                yield return new WaitForSeconds(searchDelay);

                if (!inventoryTetris.GetGrid().GetGridObject(col, row).HasPlacedObject())
                {
                    InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, defaultType);
                    continue; //if the placedGridObject is empty, then continue loop
                }

                PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(col, row).GetPlacedObject();
                PlacedObjectTypeSO itemTetrisSO = placedObject.GetPlacedObjectTypeSO();

                if (itemTetrisSO.nameString == target)
                {
                    //if we found a matching targetItemname, then use the item
                    //print("found it at: " + col + ", " + row);

                    //get reference to itemObject
                    InventoryItem item = inventoryTetris.GetGrid().GetGridObject(col, row).GetPlacedObject().GetComponent<InventoryItem>();
                    InventorySearchSystem.Instance.SetFoundItem(item);
                    //print(itemObject);

                    //after item is found, reset search state
                    searchState = false;
                    //Let GridSearchSystem know item was found
                    InventorySearchSystem.Instance.CanContinue(true);
                    InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, defaultType);
                    break;
                }
                InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, defaultType);
            }

            // hide the arrow again
            if (useArrow) { inventoryArrow.Hide(); }

            //increment row counter, so we can go down to next row
            row--;
        }
        InventorySearchSystem.Instance.CanContinue(true);
    }

    public int GetTopMostRow(InventoryTetris inventoryTetris)
    {
        for (int row = inventoryTetris.GetHeight() - 1; row >= 0; row--)
        {
            //print(row);
            for (int col = 0; col < inventoryTetris.GetWidth(); col++)
            {
                //print(col);
                if (inventoryTetris.GetGrid().GetGridObject(col, row).HasPlacedObject())
                {
                    return row;
                }
            }
        }

        return -1;
    }
}
