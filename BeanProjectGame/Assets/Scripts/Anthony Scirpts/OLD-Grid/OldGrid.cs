using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OldGrid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Transform parent;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public OldGrid(int width, int height, float cellSize, Transform parent, Vector3 originPosition, Func<OldGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.parent = parent;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
                gridArray[x, y] = createGridObject(this, x, y);


        /*for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x, y+1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
            }
        }*/
    }

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public float GetCellSize() { return cellSize; }
    public Transform GetParent() { return parent; }

    public Vector3 GetWorldPosition(float x, float y)
    {
        //print(Mathf.FloorToInt((x - originPosition.x) / cellSize) + ", " + Mathf.FloorToInt((y - originPosition.y) / cellSize));
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector3 GetCanvasWorldPosition(Canvas canvas, float x, float y, Camera cam)
    {
        Vector3 vector = GetWorldPosition(x,y);
        Vector3 output;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), vector, cam, out output);
        return output;
    }

    public void GetXYPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        //x = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
        //y = Mathf.RoundToInt((worldPosition - originPosition).y / cellSize);

        //print("[" + x + ", " + y + "]");
    }

    public void TriggerGridValueChanged(int xVal, int yVal)
    {
        OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = xVal, y = yVal });
    }

    public TGridObject GetGridCellValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            //Debug.Log(default(TGridObject));
            return default(TGridObject);
        }
    }

    public TGridObject GetGridCellValue(Vector3 worldPosition)
    {
        GetXYPosition(worldPosition, out int x, out int y);
        return GetGridCellValue(x, y);
    }

    public void SetGridCellValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXYPosition(worldPosition, out x, out y);
        SetGridCellValue(x, y, value);
    }
}