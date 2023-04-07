using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : InventoryItem
{
    #region OverrideFunctions
    public override bool PrimaryAction(GameObject player)
    {
        //PlayerInventory currentInv = player.GetComponent<PlayerInventory>();

        MenuManager.instance.LoadForestScene();

        return true;
    }

    public override void PickItemUp(Inventory targetInv)
    {
        // Do nothing, this is not an item that can be picked up
    }

    #endregion OverrideFunctions
}