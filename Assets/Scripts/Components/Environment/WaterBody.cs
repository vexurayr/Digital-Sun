using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : InventoryItem
{
    [SerializeField] private float thirstRestored;

    public override bool PrimaryAction(GameObject player)
    {
        if (!player.GetComponent<Thirst>())
        {
            return false;
        }

        Thirst thirst = player.GetComponent<Thirst>();

        thirst.IncCurrentValue(thirstRestored);

        return true;
    }

    // Inheriting from InventoryItem makes this easier, but we don't want the player to pick up water as is
    public override void PickItemUp(Inventory targetInv)
    {}
}