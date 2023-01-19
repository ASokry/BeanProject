using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private CharacterMotion characterMotion;

    private void Update()
    {
        EquipWeapon();
        SelectConsumable();
    }

    private void EquipWeapon()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject())
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(mouseGridPosition.x, mouseGridPosition.y).GetPlacedObject();
            InventoryWeapon inventoryWeapon = placedObject.GetComponent<InventoryWeapon>();

            if (inventoryWeapon != null)
            {
                characterMotion.SetWeaponObject(inventoryWeapon);
            }
        }
    }

    private void SelectConsumable()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject())
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(mouseGridPosition.x, mouseGridPosition.y).GetPlacedObject();
            InventoryConsumable inventoryConsumable = placedObject.GetComponent<InventoryConsumable>();

            if (inventoryConsumable != null)
            {
                characterMotion.SetConsumableObject(inventoryConsumable);
            }
        }
    }
}
