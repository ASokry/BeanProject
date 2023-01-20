using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPrep : MonoBehaviour
{
    public static InventoryPrep Instance { get; private set; }

    [SerializeField] private List<InventoryTetris> inventoriesToPrep = new List<InventoryTetris>();
    [SerializeField] private List<ItemTetrisSO> requiredItems = new List<ItemTetrisSO>();
    private List<PlacedObjectTypeSO> copyOfItemContainerItems = new List<PlacedObjectTypeSO>();

    //temp, will remove after playtest
    [SerializeField] private CharacterMotion characterMotion;
    [SerializeField] private Text playtestInstructions;
    [SerializeField] private Button startButton; //temporary, will remove after game loop is more established
    public bool canEquip = false;
    //

    private void Awake()
    {
        Instance = this;
        startButton.interactable = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {

            //print(IsInventoryPrepped(0));
        }

        //temp, will remove after playtest
        if (IsInventoryPrepped(0))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public bool IsInventoryPrepped(int index)
    {
        if (inventoriesToPrep.Count <= 0)
        {
            print("There are inventories to prep");
            return false;
        }

        if(index < 0 || index > inventoriesToPrep.Count)
        {
            print("index is negative or too high");
            return false;
        }

        InventoryTetris inventoryTetris = inventoriesToPrep[index];
        return CheckRequirements(inventoryTetris);
    }

    private bool CheckRequirements(InventoryTetris inventoryTetris)
    {
        if (requiredItems.Count <= 0) { return true; }

        RectTransform itemContainer = inventoryTetris.GetItemContainer();
        PlacedObject[] items = itemContainer.transform.GetComponentsInChildren<PlacedObject>();
        foreach(PlacedObject placedObject in items) { copyOfItemContainerItems.Add(placedObject.GetPlacedObjectTypeSO()); }

        int count = 0;
        foreach (ItemTetrisSO item in requiredItems)
        {
            //print(copyOfItemContainerItems.Contains(item));
            if (copyOfItemContainerItems.Contains(item))
            {
                copyOfItemContainerItems.Remove(item);
                count++;
            }
        }
        copyOfItemContainerItems.Clear();
        //print(count);
        if (count == requiredItems.Count)
        {
            return true;
        }
        return false;
    }

    //temp, will remove after playtest
    public void StartWalking()
    {
        if (characterMotion)
        {
            characterMotion.SetStop(false);
            startButton.gameObject.SetActive(false);
            canEquip = true;
            string text = "SPACE:\nspawn items\n\nLeft Click:\ndrag and drop\n\nRight Click:\nuse item or rotate";
            ChangeText(text);
        }
    }

    public void ChangeText(string text)//temp, will remove after playtest
    {
        playtestInstructions.text = text;
    }
}
