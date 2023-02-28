using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPowerup : Powerup
{
    public override void Apply(PowerupManager target)
    {
        Stamina stamina = target.GetComponent<Stamina>();
        Thirst thirst = target.GetComponent<Thirst>();

        if (stamina != null)
        {
            stamina.IncRegainMult(statChangeAmount);
        }
        if (thirst != null)
        {
            thirst.IncCurrentValue(secondStatChangeAmount);
        }
    }

    public override void Remove(PowerupManager target)
    {
        Stamina stamina = target.GetComponent<Stamina>();

        if (stamina != null)
        {
            stamina.DecRegainMult(statChangeAmount);
        }
    }
}