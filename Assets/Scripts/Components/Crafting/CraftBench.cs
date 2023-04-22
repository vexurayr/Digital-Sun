using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftBench : InventoryItem
{
    [SerializeField] private GameObject craftBenchUI;

    public override bool PrimaryAction(GameObject player)
    {
        craftBenchUI.GetComponent<CraftBenchUI>().SetInteractingObject(PlayerInventoryManager.instance.GetPlayerInventory());
        ToggleUI();
        return true;
    }

    public void ToggleUI()
    {
        if (craftBenchUI.activeInHierarchy)
        {
            craftBenchUI.SetActive(false);
        }
        else
        {
            craftBenchUI.SetActive(true);
        }
    }

    public GameObject GetCraftBenchUI()
    {
        return craftBenchUI;
    }
}