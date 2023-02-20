using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached on every inventory tetris GameObject
public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private CharacterMotion characterMotion;

    //equipped weapon values
    private InventoryWeapon currentEquippedObject;
    private List<Vector2Int> equippedPositionList;

    public InventoryWeapon GetCurrentEquippedObject() { return currentEquippedObject; }
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
            if (placedObject == null) return;
            InventoryWeapon inventoryWeapon = placedObject.GetComponent<InventoryWeapon>();

            if (inventoryWeapon != null && inventoryWeapon != currentEquippedObject)
            {
                //reset last Equipped Weapon;
                ResetEquippedWeapon();

                //set overlay of newly equipped weapon
                InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
                InventoryTileSystem.TileOverlayType tileOverlayType = InventoryTileSystem.TileOverlayType.Equipped;
                Vector2Int origin = placedObject.GetGridPosition();
                PlacedObjectTypeSO.Dir direction = placedObject.GetDir();
                List<Vector2Int> positions = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(origin, direction);
                InventoryTileSystem.Instance.SetMultiTileOverlay(inventoryTetrisBackground, positions, tileOverlayType);
                InventoryTileSystem.Instance.SetOverlayTypeAt(inventoryTetrisBackground, positions, tileOverlayType);

                //Set equipped state of scirpt and weapon
                inventoryWeapon.SetEquippedState(true);
                currentEquippedObject = inventoryWeapon;
                equippedPositionList = positions;

                //Send reference of equipped weapon to player script
                if (characterMotion) characterMotion.SetWeaponObject(inventoryWeapon);
            }
            else if (inventoryWeapon != null && inventoryWeapon == currentEquippedObject)
            {
                //reset last Equipped Weapon;
                ResetEquippedWeapon();
            }
        }
    }

    public void UpdateEquippedWeaponPosition(List<Vector2Int> positionList)
    {
        if(positionList != null)
        {
            ResetLastEquippedOverlay();
            equippedPositionList = positionList;
            InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
            InventoryTileSystem.TileOverlayType tileOverlayType = InventoryTileSystem.TileOverlayType.Equipped;
            InventoryTileSystem.Instance.SetMultiTileOverlay(inventoryTetrisBackground, equippedPositionList, tileOverlayType);
            InventoryTileSystem.Instance.SetOverlayTypeAt(inventoryTetrisBackground, equippedPositionList, tileOverlayType);
        }
    }

    public void ResetEquippedWeapon()
    {
        ResetLastEquippedOverlay();
        if (currentEquippedObject != null)
        {
            currentEquippedObject.SetEquippedState(false);
            currentEquippedObject = null;
            if (characterMotion) characterMotion.SetWeaponObject(null);
        }
        else
        {
            //print("There is no last equipped PlacedObject");
        }
    }

    public void ResetLastEquippedOverlay()
    {
        if (equippedPositionList != null)
        {
            InventoryTetrisBackground inventoryTetrisBackground = inventoryTetris.GetInventoryTetrisBackground();
            InventoryTileSystem.TileOverlayType defualtType = InventoryTileSystem.TileOverlayType.Default;
            InventoryTileSystem.Instance.SetMultiTileOverlay(inventoryTetrisBackground, equippedPositionList, defualtType);
            InventoryTileSystem.Instance.SetOverlayTypeAt(inventoryTetrisBackground, equippedPositionList, defualtType);
            equippedPositionList = null;
        }
        else
        {
            //print("There is no last equipped PlacedObject");
        }
    }

    private void SelectConsumable()
    {
        if (Input.GetMouseButtonDown(1) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject() /*&& InventoryPrep.Instance.canEquip*/)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryTetris.GetItemContainer(), Input.mousePosition, null, out Vector2 anchoredPosition);
            Vector2Int mouseGridPosition = inventoryTetris.GetGridPosition(anchoredPosition);
            PlacedObject placedObject = inventoryTetris.GetGrid().GetGridObject(mouseGridPosition.x, mouseGridPosition.y).GetPlacedObject();
            if (placedObject == null) return;
            InventoryConsumable inventoryConsumable = placedObject.GetComponent<InventoryConsumable>();

            if (inventoryConsumable != null)
            {
                if (characterMotion) characterMotion.SetConsumableObject(inventoryConsumable);
            }
        }
    }
}
