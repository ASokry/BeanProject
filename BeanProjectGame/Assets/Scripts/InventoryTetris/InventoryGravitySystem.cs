using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGravitySystem : MonoBehaviour
{
    public static InventoryGravitySystem Instance { get; private set; }

    public delegate void GravityAction();
    public static event GravityAction OnGravity;

    public enum GravitySystemState { Selecting, Starting, Processing, Finalizing, Reseting }
    [SerializeField] private GravitySystemState currentState = GravitySystemState.Selecting;

    [SerializeField] private List<InventoryGravity> gravityList = new List<InventoryGravity>();
    private InventoryGravity currentInventoryGravity;

    [SerializeField] private float gravityDelay = 0.05f;

    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(InventoryGravity inventoryGravity)
    {
        if (!gravityList.Contains(inventoryGravity))
        {
            //print("add to gravity list");
            gravityList.Add(inventoryGravity);
        }
    }

    public void RemoveFromList(int index)
    {
        //print("remove from gravity list");
        gravityList.RemoveAt(index);
    }

    private void Update()
    {
        if (currentState == GravitySystemState.Selecting && gravityList.Count > 0)
        {
            currentInventoryGravity = gravityList[0];
            currentState = GravitySystemState.Starting;
        }

        if (currentState == GravitySystemState.Starting && gravityList.Count > 0)
        {
            if (currentInventoryGravity.IsGravityOn())
            {
                //print(currentInventoryGravity.name);
                StartCoroutine(Gravity(currentInventoryGravity));
                currentState = GravitySystemState.Processing;
            }
            else
            {
                print("not affected by gravity");
                RemoveFromList(0);
                currentState = GravitySystemState.Finalizing;
                return;
            }
        }

        if (currentState == GravitySystemState.Finalizing)
        {
            //print("finalizing");
            if (OnGravity != null) OnGravity();
            currentState = GravitySystemState.Selecting;
        }
    }

    private IEnumerator Gravity(InventoryGravity inventoryGravity)
    {
        PlacedObject placedObject = inventoryGravity.GetPlacedObject();
        PlacedObjectTypeSO.Dir dir = placedObject.GetDir();
        InventoryTetris inventoryTetris = inventoryGravity.GetInventoryTetris();
        Vector2Int rowBelow = inventoryGravity.GetRowBelow();

        List<Vector2Int> gridPositionList = placedObject.GetPlacedObjectTypeSO().GetGridPositionList(rowBelow, dir);
        bool canPlace = inventoryTetris.CheckBuildItemPositions(gridPositionList, placedObject);
        //print(canPlace);

        if (canPlace)
        {
            //print(inventoryGravity.name);
            inventoryTetris.RemoveItemAt(placedObject.GetGridPosition());
            inventoryTetris.TryPlaceItem(placedObject.GetPlacedObjectTypeSO() as ItemTetrisSO, rowBelow, dir);
        }
        yield return new WaitForSeconds(gravityDelay);

        RemoveFromList(0);
        currentState = GravitySystemState.Finalizing;
    }
}
