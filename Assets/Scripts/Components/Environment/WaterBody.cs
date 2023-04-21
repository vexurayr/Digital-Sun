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
}