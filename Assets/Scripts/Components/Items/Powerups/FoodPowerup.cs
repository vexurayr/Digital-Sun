using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPowerup : Powerup
{
    // Refill target's hunger bar
    public override void ApplyPrimaryEffect(PowerupManager target)
    {
        // Gets Hunger Component
        Hunger hunger = target.GetComponent<Hunger>();

        if (hunger != null)
        {
            hunger.IncCurrentValue(statChangeAmount);
        }
    }

    // Won't be removing hunger when the powerup's timer reaches 0
    public override void RemovePrimaryEffect(PowerupManager target)
    { }
}