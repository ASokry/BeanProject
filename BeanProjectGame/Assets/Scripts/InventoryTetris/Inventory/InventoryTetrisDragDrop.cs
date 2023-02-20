/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTetrisDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private InventoryTetris inventoryTetris;
    private PlacedObject placedObject;

    private void Awake() {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        placedObject = GetComponent<PlacedObject>();
    }

    public void Setup(InventoryTetris inventoryTetris) {
        this.inventoryTetris = inventoryTetris;
    }

    //To be filled with requirements allowing item to be dragged
    private bool CanItemBeDragged()
    {
        bool searchInProgress = InventorySearchSystem.Instance.SearchState();
        InventoryWeapon inventoryWeapon = GetComponent<InventoryWeapon>();
        bool isItemEquipped = inventoryWeapon ? !inventoryWeapon.GetEquippedState() : true;

        bool result = !searchInProgress && isItemEquipped;

        return result;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (Input.GetMouseButton(0) && !InventoryTetrisDragDropSystem.Instance.GetPlacedObject())
        {
            if (CanItemBeDragged())
            {
                //Debug.Log("OnBeginDrag");
                canvasGroup.alpha = .7f;
                canvasGroup.blocksRaycasts = false;

                //ItemTetrisSO.CreateVisualGrid(transform.GetChild(0), placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, inventoryTetris.GetGrid().GetCellSize());

                InventoryTetrisDragDropSystem.Instance.StartedDragging(inventoryTetris, placedObject);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("OnDrag");
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (Input.GetMouseButtonUp(0) && InventoryTetrisDragDropSystem.Instance.GetPlacedObject())
        {
            //Debug.Log("OnEndDrag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            InventoryTetrisDragDropSystem.Instance.StoppedDragging(inventoryTetris, placedObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("OnPointerDown");
    }

}
