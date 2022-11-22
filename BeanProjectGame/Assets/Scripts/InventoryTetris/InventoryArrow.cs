using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryArrow : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;

    private void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Reveal()
    {
        gameObject.SetActive(true);
    }

    public void MoveArrow(int x, int y)
    {
        Vector3 position = inventoryTetris.GetGrid().GetWorldPosition(x, y);
        //print(position);
        GetComponent<RectTransform>().anchoredPosition = position;
    }
}
