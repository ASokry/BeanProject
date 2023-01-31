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

    //[SerializeField] private InventoryTetrisBackground inventoryTetrisBackground;

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
        int row = GetStartingRow(inventoryTetris);
        InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
        InventoryTileSystem.TileOverlayType searchType = InventoryTileSystem.TileOverlayType.Search;

        while (row >= 0 && searchState)
        {
            //Reveal and move the arrow along y axis of grid on left hand side
            if (useArrow)
            {
                inventoryArrow.SetMax(inventoryTetris.GetWidthMax());
                inventoryArrow.ResetFill();
                inventoryArrow.MoveArrow(0, row);
                inventoryArrow.Reveal();
            }

            // search through each column of current row
            int column = GetStartingCol(inventoryTetris, row);
            if (column < 0) { /*print("there are no columns to traverse");*/ break; }
            for (int col = column; col < inventoryTetris.GetWidthMax(); col++)
            {
                Vector2Int coordinate = new Vector2Int(col, row);
                if (CheckIfTileIsNull(coordinate.x, coordinate.y)) continue;// if coordinates are null, then continue loop

                //save original tile overlay type, to reset tile after tile has been search
                InventoryTileSystem.TileOverlayType originalType = InventoryTileSystem.Instance.CurrentOverlayTypeAt(inventoryTetrisBackground, coordinate);

                //Change tile overlay type to searching
                InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, searchType);
                
                if (useArrow) { inventoryArrow.Fill(); }
                yield return new WaitForSeconds(searchDelay);

                if (!inventoryTetris.GetGrid().GetGridObject(col, row).HasPlacedObject())
                {
                    InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, originalType);
                    continue; //if the placedGridObject is empty, then continue loop
                }

                PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(col, row).GetPlacedObject();
                PlacedObjectTypeSO itemTetrisSO = placedObject.GetPlacedObjectTypeSO();
                //print(itemTetrisSO.nameString + ", " + target);
                if (itemTetrisSO.nameString == target)
                {
                    //if we found a matching targetItemname, then use the item
                    //print("found it at: " + col + ", " + row);

                    //get reference to itemObject
                    InventoryItem item = inventoryTetris.GetGrid().GetGridObject(col, row).GetPlacedObject().GetComponent<InventoryItem>();
                    Vector2Int itemCoordinates = new Vector2Int(col, row);
                    InventorySearchSystem.Instance.SetFoundItem(item, inventoryTetris, itemCoordinates);
                    //print(itemObject);

                    //after item is found, reset search state
                    searchState = false;
                    //Let GridSearchSystem know item was found
                    InventorySearchSystem.Instance.CanContinue(true);
                    InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, originalType);
                    break;
                }
                InventoryTileSystem.Instance.SetTileOverlay(inventoryTetrisBackground, coordinate, originalType);
            }

            // hide the arrow again
            if (useArrow) { inventoryArrow.Hide(); }

            //increment row counter, so we can go down to next row
            row--;
        }
        InventorySearchSystem.Instance.CanContinue(true);
    }

    private int GetStartingRow(InventoryTetris inventoryTetris)
    {
        for (int row = inventoryTetris.GetHeightMax() - 1; row >= 0; row--)
        {
            //print(row);
            for (int col = 0; col < inventoryTetris.GetWidthMax(); col++)
            {
                //print(col);
                if (CheckIfTileIsNull(col, row)) continue;
                if (inventoryTetris.GetGrid().GetGridObject(col, row).HasPlacedObject())
                {
                    return row;
                }
            }
        }
        return -1;
    }

    private int GetStartingCol(InventoryTetris inventoryTetris, int row)
    {
        for (int col = 0; col < inventoryTetris.GetWidthMax(); col++)
        {
            //print(col);
            if (CheckIfTileIsNull(col, row)) continue;
            return col;
        }
        return -1;
    }

    private bool CheckIfTileIsNull(int x, int y)
    {
        Vector2Int coordinate = new Vector2Int(x, y);
        InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
        //print(InventoryTileSystem.Instance.IsTileNull(inventoryTetrisBackground, coordinate));
        return InventoryTileSystem.Instance.IsTileNull(inventoryTetrisBackground, coordinate);
    }
}
