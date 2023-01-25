using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached on every inventory tetris GameObject
public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private CharacterMotion characterMotion;
    private InventoryWeapon currentEquippedPlacedObject;

    //Last equipped weapon values
    private List<Vector2Int> equippedPositionList;

    private void Update()
    {
        EquipWeapon();
        SelectConsumable();
    }

    private void EquipWeapon()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject() /*&& InventoryPrep.Instance.canEquip*/)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(mouseGridPosition.x, mouseGridPosition.y).GetPlacedObject();
            InventoryWeapon inventoryWeapon = placedObject.GetComponent<InventoryWeapon>();

            if (inventoryWeapon != null && inventoryWeapon != currentEquippedPlacedObject)
            {
                //reset last Equipped Weapon;
                ResetLastEquipped();

                //set overlay of newly equipped weapon
                InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
                InventoryTileSystem.TileOverlayType tileOverlayType = InventoryTileSystem.TileOverlayType.Equipped;
                Vector2Int origin = placedObject.GetGridPosition();
                PlacedObjectTypeSO.Dir direction = placedObject.GetDir();
                List<Vector2Int> positions = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(origin, direction);
                InventoryTileSystem.Instance.SetMultiTileOverlay(inventoryTetrisBackground, positions, tileOverlayType);

                //Set equipped state of scirpt and weapon
                inventoryWeapon.SetEquippedState(true);
                currentEquippedPlacedObject = inventoryWeapon;
                equippedPositionList = positions;

                //Send reference of equipped weapon to player script
                if (characterMotion) characterMotion.SetWeaponObject(inventoryWeapon);
            }
            else if (inventoryWeapon != null && inventoryWeapon == currentEquippedPlacedObject)
            {
                //reset last Equipped Weapon;
                ResetLastEquipped();
            }
        }
    }

    public void ResetLastEquipped()
    {
        ResetLastEquippedOverlay();
        if (currentEquippedPlacedObject != null)
        {
            currentEquippedPlacedObject.SetEquippedState(false);
            currentEquippedPlacedObject = null;
            if (characterMotion) characterMotion.SetWeaponObject(null);
        }
        else
        {
            print("There is no last equipped PlacedObject");
        }
    }

    public void ResetLastEquippedOverlay()
    {
        if (equippedPositionList != null)
        {
            InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
            InventoryTileSystem.TileOverlayType defualtType = InventoryTileSystem.TileOverlayType.Default;
            InventoryTileSystem.Instance.SetMultiTileOverlay(inventoryTetrisBackground, equippedPositionList, defualtType);
            equippedPositionList = null;
        }
        else
        {
            print("There is no last equipped PlacedObject");
        }
    }

    private void SelectConsumable()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject() /*&& InventoryPrep.Instance.canEquip*/)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(mouseGridPosition.x, mouseGridPosition.y).GetPlacedObject();
            InventoryConsumable inventoryConsumable = placedObject.GetComponent<InventoryConsumable>();

            if (inventoryConsumable != null)
            {
                if (characterMotion) characterMotion.SetConsumableObject(inventoryConsumable);
            }
        }
    }
}
