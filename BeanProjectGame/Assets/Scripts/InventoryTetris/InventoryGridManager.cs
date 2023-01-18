using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridManager : MonoBehaviour
{
    public static InventoryGridManager Instance { get; private set; }
    public enum InventoryState { Defualt, Locked, Upgrading}
    [SerializeField] private InventoryState currentState = InventoryState.Defualt;

    [SerializeField] private GameObject upgradeUI;
    private Button upgradeButton;
    [SerializeField] private int startingUpgradePoints = 0;
    [SerializeField] private int currentUpgradePoints = 0;

    private List<InventoryTetris> inventoryTetrisList = new List<InventoryTetris>();

    public InventoryState GetCurrentState() { return currentState; }
    public void SetCurrentState(InventoryState state) { currentState = state; }
    public int CheckUpgradePoints() { return currentUpgradePoints; }
    public void SetUpgradePoints(int points) { currentUpgradePoints = points > 0 ? points : 0; }
    public void SetStartingUpgradePoints(int points)
    {
        startingUpgradePoints = points > 0 ? points : 0;
        currentUpgradePoints = startingUpgradePoints;
    }

    public void AddToManager(InventoryTetris inventoryTetris) { inventoryTetrisList.Add(inventoryTetris); }

    private void Awake()
    {
        Instance = this;
        upgradeButton = upgradeUI.transform.Find("Finish Upgrades").GetComponent<Button>();
        upgradeButton.interactable = false;
        upgradeUI.SetActive(false);
    }

    private void Update()
    {
        if (currentState == InventoryState.Upgrading)
        {
            upgradeUI.SetActive(true);
            if (currentUpgradePoints == 0)
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
        else
        {
            upgradeUI.SetActive(false);
        }
    }

    public void UseUpgradePoints()
    {
        if (currentState == InventoryState.Upgrading)
        {
            if (currentUpgradePoints == 0)
            {
                foreach (InventoryTetris inventoryTetris in inventoryTetrisList)
                {
                    inventoryTetris.UpgradeTiles();
                }
            }
            currentState = InventoryState.Defualt; //temporary, may change to different state depending on gameplay loop
        }
    }

    public void ResetUpgradePoints()
    {
        if (currentState == InventoryState.Upgrading)
        {
            currentUpgradePoints = startingUpgradePoints;
            foreach (InventoryTetris inventoryTetris in inventoryTetrisList)
            {
                inventoryTetris.ResetUpgradeables();
            }
        }
    }
}
