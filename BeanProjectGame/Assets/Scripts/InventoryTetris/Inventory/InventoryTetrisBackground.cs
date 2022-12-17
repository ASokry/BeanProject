using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTetrisBackground : MonoBehaviour {

    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTile inventoryTile;

    private Dictionary<Vector2Int, InventoryTile> inventoryTileDictionary = new Dictionary<Vector2Int, InventoryTile>();
    public Dictionary<Vector2Int, InventoryTile> GetInventoryTileDictionary() { return inventoryTileDictionary; }

    private void Start() {
        // Create background
        //Transform template = transform.Find("Template");
        //template.gameObject.SetActive(false);
        inventoryTile.gameObject.SetActive(false);

        for (int x = 0; x < inventoryTetris.GetGrid().GetWidth(); x++) {
            for (int y = 0; y < inventoryTetris.GetGrid().GetHeight(); y++) {
                Transform backgroundSingleTransform = Instantiate(inventoryTile.transform, transform);
                InventoryTile tile = backgroundSingleTransform.GetComponent<InventoryTile>();
                tile.Setup(inventoryTetris);
                inventoryTileDictionary.Add(new Vector2Int(x,y), tile);
                backgroundSingleTransform.gameObject.SetActive(true);
            }
        }

        GetComponent<GridLayoutGroup>().cellSize = new Vector2(inventoryTetris.GetGrid().GetCellSize(), inventoryTetris.GetGrid().GetCellSize());

        GetComponent<RectTransform>().sizeDelta = new Vector2(inventoryTetris.GetGrid().GetWidth(), inventoryTetris.GetGrid().GetHeight()) * inventoryTetris.GetGrid().GetCellSize();

        GetComponent<RectTransform>().anchoredPosition = inventoryTetris.GetComponent<RectTransform>().anchoredPosition;
    }

}