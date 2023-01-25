using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTileSystem : MonoBehaviour
{
    public static InventoryTileSystem Instance { get; private set; }

    public enum TileType { None, Default, Null, Upgrade}
    [SerializeField] private Sprite upgradeTile;

    public enum TileOverlayType { None, Default, Search, Upgradeable, Equipped}
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
                tile.SetColor(Color.cyan, tileOverlay);
                tile.SetOpacity(overlayOpacity);
                break;
            case TileOverlayType.Upgradeable:
                tile.SetColor(Color.magenta, tileOverlay);
                tile.SetOpacity(overlayOpacity);
                break;
            case TileOverlayType.Equipped:
                tile.SetColor(Color.yellow, tileOverlay);
                tile.SetOpacity(overlayOpacity);
                break;
            default:
                tile.ResetColor();
                break;
        }
    }

    public void SetMultiTileOverlay(InventoryTetrisBackground inventoryTetrisBackground, List<Vector2Int> coordinates, TileOverlayType tileOverlay)
    {
        foreach (Vector2Int coordinate in coordinates) SetTileOverlay(inventoryTetrisBackground, coordinate, tileOverlay);
    }

    public TileOverlayType CurrentOverlayTypeAt(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinate)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinate, out InventoryTile tile);
        if (tile == null)
        {
            //print("Coordinates does not exsist: " + coordinates);
            return TileOverlayType.None;
        }
        return tile.CurrentOverlayType();
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
                tile.SetImageSprite(upgradeTile, tileType);
                break;
            default:
                tile.ResetSprite();
                break;
        }
    }

    public TileType CurrentTileTypeAt(InventoryTetrisBackground inventoryTetrisBackground, Vector2Int coordinate)
    {
        inventoryTetrisBackground.GetInventoryTileDictionary().TryGetValue(coordinate, out InventoryTile tile);
        if (tile == null)
        {
            //print("Coordinates does not exsist: " + coordinates);
            return TileType.None;
        }
        return tile.CurrentSpriteType();
    }
}
