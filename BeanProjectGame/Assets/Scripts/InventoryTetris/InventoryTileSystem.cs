using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTileSystem : MonoBehaviour
{
    public static InventoryTileSystem Instance { get; private set; }

    public enum TileType { Default, Null, Upgrade}
    [SerializeField] private Sprite upgradeTile;

    public enum TileOverlayType { Default, Search, Upgradeable}
    [SerializeField, Range(0, 1)] private float overlayOpacity = 0.5f;
    

    private void Awake()
    {
        Instance = this;
    }

    public bool IsTileNull(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if (tile == null)
        {
            //print("Coordinates does not exsist: " + coordinates);
            return true;
        }
        return tile.IsNull();
    }

    public bool IsTileUpgradeable(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if (tile == null)
        {
            //print("Coordinates does not exsist: " + coordinates);
            return false;
        }
        return tile.IsUpgradeable();
    }

    public void SetTileUpgradeable(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates, bool b)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if (tile == null)
        {
            //print("Coordinates does not exsist: " + coordinates);
            return;
        }
        tile.SetUpgradeable(b);
    }

    public void SetTileOverlay(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates, TileOverlayType tileOverlay)
    {
        inventoryTetrisBackground.GetInventoryOverlayDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if(tile == null || tile.IsNull())
        {
            print("Overlay Coordinates does not exsist, " + coordinates);
            return;
        }

        switch (tileOverlay)
        {
            case TileOverlayType.Default:
                tile.ResetColor();
                break;
            case TileOverlayType.Search:
                tile.SetColor(Color.cyan);
                tile.SetOpacity(overlayOpacity);
                break;
            case TileOverlayType.Upgradeable:
                tile.SetColor(Color.yellow);
                tile.SetOpacity(overlayOpacity);
                break;
            default:
                tile.ResetColor();
                break;
        }
    }

    public void SetTileSprite(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinates, TileType tileType)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinates, out InventoryTile tile);
        if (tile == null)
        {
            print("Coordinates does not exsist, " + coordinates);
            return;
        }

        switch (tileType)
        {
            case TileType.Default:
                tile.ResetSprite();
                break;
            case TileType.Null:
                tile.SetNull();
                break;
            case TileType.Upgrade:
                tile.SetImageSprite(upgradeTile);
                break;
            default:
                tile.ResetSprite();
                break;
        }
    }
}
