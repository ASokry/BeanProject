using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour
{
    [SerializeField] private bool isNullTile = false;
    [SerializeField] private bool canBeUpgraded = false;
    private InventoryTetris inventoryTetris;
    private Image image;
    private Sprite defaultSprite;
    private InventoryTileSystem.TileType currentType;
    
    //overlay variables
    private Color defaultOverlay = Color.white;
    private InventoryTileSystem.TileOverlayType currentOverlayType;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
        currentType = InventoryTileSystem.TileType.Default;
        defaultOverlay.a = 0;
        currentOverlayType = InventoryTileSystem.TileOverlayType.Default;
    }
    public InventoryTileSystem.TileType CurrentSpriteType() { return currentType; }
    public void SetOverlayType(InventoryTileSystem.TileOverlayType tileOverlayType) { currentOverlayType = tileOverlayType; }
    public InventoryTileSystem.TileOverlayType CurrentOverlayType() { return currentOverlayType; }

    public void Setup(InventoryTetris inventoryTetris)
    {
        this.inventoryTetris = inventoryTetris;
    }

    public bool IsNull()
    {
        return isNullTile;
    }

    public void SetNull()
    {
        isNullTile = true;
        image.sprite = null;
        //image.sprite = defaultSprite;
        //image.enabled = false;
    }

    public bool IsUpgradeable()
    {
        return canBeUpgraded;
    }

    public void SetUpgradeable(bool b)
    {
        canBeUpgraded = b;
    }

    public void SetImageSprite(Sprite sprite, InventoryTileSystem.TileType type)
    {
        image.enabled = true;
        image.sprite = sprite;
        currentType = type;
    }

    public void ResetSprite()
    {
        isNullTile = false;
        canBeUpgraded = false;
        image.enabled = true;
        image.sprite = defaultSprite;
        currentType = InventoryTileSystem.TileType.Default;
    }

    public void SetColor(Color color, InventoryTileSystem.TileOverlayType overlayType)
    {
        image.enabled = true;
        image.color = color;
        currentOverlayType = overlayType;
    }

    public void SetOpacity(float opacity)
    {
        Color color = image.color;
        color.a = Mathf.Clamp01(opacity);
        image.color = color;
    }

    public void ResetColor()
    {
        image.enabled = true;
        image.color = defaultOverlay;
        currentOverlayType = InventoryTileSystem.TileOverlayType.Default;
    }
}
