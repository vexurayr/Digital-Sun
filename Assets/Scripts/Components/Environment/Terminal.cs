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

    #endregion OverrideFunctions
}