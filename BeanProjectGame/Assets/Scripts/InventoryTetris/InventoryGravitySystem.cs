using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGravitySystem : MonoBehaviour
{
    public static InventoryGravitySystem Instance { get; private set; }

    public enum GravitySystemState { Selecting, Starting, Processing, Finalizing, Reseting }
    [SerializeField] private GravitySystemState currentState = GravitySystemState.Selecting;

    [SerializeField] private List<InventoryGravity> gravityList = new List<InventoryGravity>();
    private InventoryGravity currentInventoryGravity;

    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(InventoryGravity inventoryGravity)
    {
        gravityList.Add(inventoryGravity);
    }

    public void RemoveFromList(int index)
    {
        gravityList.RemoveAt(index);
        currentState = GravitySystemState.Selecting;
    }

    private void Update()
    {
        if (currentState == GravitySystemState.Selecting && gravityList.Count > 0)
        {
            currentInventoryGravity = gravityList[0];
            currentState = GravitySystemState.Processing;
        }

        if (currentState == GravitySystemState.Processing)
        {
            currentInventoryGravity.Gravity();
        }
    }
}
