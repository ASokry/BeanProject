using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTetrisBackground : MonoBehaviour {

    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTile inventoryTile;

    [SerializeField] private bool overlay;
    [SerializeField] private Transform inventoryOverlay;
    [SerializeField] private InventoryTile tileOverlay;

    private Dictionary<Vector2Int, InventoryTile> inventoryTileDictionary = new Dictionary<Vector2Int, InventoryTile>();
    private Dictionary<Vector2Int, InventoryTile> inventoryOverlayDictionary = new Dictionary<Vector2Int, InventoryTile>();
    public Dictionary<Vector2Int, InventoryTile> GetInventoryTileDictionary() { return inventoryTileDictionary; }
    public Dictionary<Vector2Int, InventoryTile> GetInventoryOverlayDictionary() { return inventoryOverlayDictionary; }

    private void Start() {
        if (overlay && (!inventoryOverlay || !tileOverlay)) Debug.LogError("Overlay reference is missing!");
        // Create background
        //Transform template = transform.Find("Template");
        //template.gameObject.SetActive(false);
        inventoryTile.gameObject.SetActive(false);

        for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++) {
            for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++) {
                //Normal Grid background
                Vector2Int coordinate = new Vector2Int(y, x);
                Transform backgroundSingleTransform = Instantiate(inventoryTile.transform, transform);
                backgroundSingleTransform.name += coordinate;
                InventoryTile tile = backgroundSingleTransform.GetComponent<InventoryTile>();
                tile.Setup(inventoryTetris);
                inventoryTileDictionary.Add(coordinate, tile);
                backgroundSingleTransform.gameObject.SetActive(true);

                //Grid Overlay
                if (overlay && tileOverlay)
                {
                    Transform overlayTransform = Instantiate(tileOverlay.transform, inventoryOverlay);
                    overlayTransform.name += coordinate;
                    InventoryTile overlayTile = overlayTransform.GetComponent<InventoryTile>();
                    overlayTile.Setup(inventoryTetris);
                    inventoryOverlayDictionary.Add(coordinate, overlayTile);
                    overlayTransform.gameObject.SetActive(true);
                }
            }
        }

        GetComponent<GridLayoutGroup>().cellSize = new Vector2(inventoryTetris.GetGrid().GetCellSize(), inventoryTetris.GetGrid().GetCellSize());

        GetComponent<RectTransform>().sizeDelta = new Vector2(inventoryTetris.GetGrid().GetWidth(), inventoryTetris.GetGrid().GetHeight()) * inventoryTetris.GetGrid().GetCellSize();

        GetComponent<RectTransform>().anchoredPosition = inventoryTetris.GetComponent<RectTransform>().anchoredPosition;
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            print(mouseGridPosition);
            //print(inventoryTetris.GetList().Contains(mouseGridPosition));
            print(inventoryTileDictionary[mouseGridPosition].GetImage().sprite == null);
        }
    }*/
}