using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellValue
{
    private Grid<GridCellValue> grid;
    private int x;
    private int y;
    private PlacedGridObject placedGridObject;

    public GridCellValue(Grid<GridCellValue> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetPlacedGridObject(PlacedGridObject placedGridObject)
    {
        this.placedGridObject = placedGridObject;
        grid.TriggerGridValueChanged(x, y);
    }

    public PlacedGridObject GetPlacedGridObject() { return placedGridObject; }

    public void ClearPlacedGridObject()
    {
        placedGridObject = null;
        grid.TriggerGridValueChanged(x, y);
    }

    public bool IsPlacedGridObjectEmpty() { return placedGridObject == null; }
    public override string ToString() { return x + ", " + y + "\n" + placedGridObject; }
}
