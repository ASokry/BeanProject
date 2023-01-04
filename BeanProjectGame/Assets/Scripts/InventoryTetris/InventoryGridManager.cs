using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGridManager : MonoBehaviour
{
    public static InventoryGridManager Instance { get; private set; }
    public enum InventoryState { Defualt, Locked, Upgrading}
    [SerializeField] private InventoryState currentState = InventoryState.Defualt;
    [SerializeField] private int upgradePoints = 0;

    public InventoryState GetCurrentState() { return currentState; }
    public void SetCurrentState(InventoryState state) { currentState = state; }
    public int CheckUpgradePoints() { return upgradePoints; }
    public void SetUpgradePoints(int points) { upgradePoints = points > 0 ? points : 0; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (upgradePoints <= 0) currentState = InventoryState.Defualt;
    }
}
