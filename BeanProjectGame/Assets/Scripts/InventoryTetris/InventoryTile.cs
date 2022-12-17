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

    private void Start()
    {
        image = GetComponent<Image>();
        defaultSprite = image.sprite;
    }

    public void Setup(InventoryTetris inventoryTetris)
    {
        this.inventoryTetris = inventoryTetris;
    }

    public void SetNull()
    {
        isNullTile = true;
        image.sprite = null;
    }

    public void SetImageSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void ResetSprite()
    {
        image.sprite = defaultSprite;
    }
}
