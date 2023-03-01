using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{
    // This will give the target the healing amount to their health component
    public override void ApplyPrimaryEffect(PowerupManager target)
    {
        // Gets Health Component
        Health health = target.GetComponent<Health>();

        // Only heals if it has a health component
        if (health != null)
        {
            health.IncCurrentValue(statChangeAmount);
        }
    }

    // We don't want to take health away, so this is empty
    // The powerup can be safely removed from the PowerupManager
    public override void RemovePrimaryEffect(PowerupManager target)
    {}
}