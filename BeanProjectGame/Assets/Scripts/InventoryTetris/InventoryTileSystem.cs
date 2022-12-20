using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTileSystem : MonoBehaviour
{
    public static InventoryTileSystem Instance { get; private set; }

    public enum TileType { Default, Search, Null}
    [SerializeField] private Sprite searchTile;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTile(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates, TileType tileType)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if(tile == null)
        {
            print("Coordinates does not exsist, " + coordinates);
            return;
        }

        switch (tileType)
        {
            case TileType.Default:
                tile.ResetSprite();
                break;
            case TileType.Search:
                tile.SetImageSprite(searchTile);
                break;
            case TileType.Null:
                tile.SetNull();
                break;
            default:
                tile.ResetSprite();
                break;
        }
    }
}
