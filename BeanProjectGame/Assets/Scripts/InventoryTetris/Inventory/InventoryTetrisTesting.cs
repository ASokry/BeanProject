using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTetrisTesting : MonoBehaviour {

    [SerializeField] private Transform outerInventoryTetrisBackground;
    [SerializeField] private InventoryTetris inventoryTetris;
    [SerializeField] private InventoryTetris outerInventoryTetris;
    [SerializeField] private List<string> addItemTetrisSaveList;

    private int addItemTetrisSaveListIndex;

    private void Start() {
        outerInventoryTetrisBackground.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            outerInventoryTetrisBackground.gameObject.SetActive(true);
            outerInventoryTetris.SetActiveGrid(true);
            outerInventoryTetris.Load(addItemTetrisSaveList[addItemTetrisSaveListIndex]);

            addItemTetrisSaveListIndex = (addItemTetrisSaveListIndex + 1) % addItemTetrisSaveList.Count;

            InventoryPrep.Instance.ChangeText("Drag all items into left Grid");//temp, will remove after playtest
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log(inventoryTetris.Save());
        }
    }
}
