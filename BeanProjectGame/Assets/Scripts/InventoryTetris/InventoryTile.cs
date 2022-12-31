using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTile : MonoBehaviour
{
    [SerializeField] private bool isNullTile = false;
    private InventoryTetris inventoryTetris;
    private Image image;
    private Sprite defaultSprite;

    public Image GetImage()
    {
        return image;
    }

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
        //image.enabled = false;
    }

    public void SetImageSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void ResetSprite()
    {
        isNullTile = false;
        image.sprite = defaultSprite;
    }

    public void SetColor(Color color)
    {
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
        Color defualtColor = Color.white;
        defualtColor.a = 0;
        image.color = defualtColor;
    }
}
