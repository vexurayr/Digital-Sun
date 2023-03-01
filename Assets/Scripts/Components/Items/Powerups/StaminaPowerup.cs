using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPowerup : Powerup
{
    public override void ApplyPrimaryEffect(PowerupManager target)
    {
        Stamina stamina = target.GetComponent<Stamina>();

        if (stamina != null)
        {
            stamina.IncRegainMult(statChangeAmount);
        }
    }

    public override void ApplySecondEffect(PowerupManager target)
    {
        Thirst thirst = target.GetComponent<Thirst>();

        if (thirst != null)
        {
            thirst.IncCurrentValue(secondStatChangeAmount);
        }
    }

    public override void RemovePrimaryEffect(PowerupManager target)
    {
        Stamina stamina = target.GetComponent<Stamina>();

        if (stamina != null)
        {
            stamina.DecRegainMult(statChangeAmount);
        }
    }
}