using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstPowerup : Powerup
{
    public override void ApplyPrimaryEffect(PowerupManager target)
    {
        Thirst thirst = target.GetComponent<Thirst>();

        if (thirst != null)
        {
            thirst.IncCurrentValue(statChangeAmount);
        }
    }

    public override void RemovePrimaryEffect(PowerupManager target)
    { }
}