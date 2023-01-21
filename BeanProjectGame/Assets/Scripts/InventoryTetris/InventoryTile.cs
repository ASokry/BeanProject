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

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }

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

    public void SetImageSprite(Sprite sprite)
    {
        image.enabled = true;
        image.sprite = sprite;
    }

    public void ResetSprite()
    {
        isNullTile = false;
        canBeUpgraded = false;
        image.enabled = true;
        image.sprite = defaultSprite;
    }

    public void SetColor(Color color)
    {
        image.enabled = true;
        image.color = color;
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
        Color defualtColor = Color.white;
        defualtColor.a = 0;
        image.color = defualtColor;
    }
}
