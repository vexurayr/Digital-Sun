using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class HealingItem : InventoryItem
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] private float healthBarRestore;
    [SerializeField] private float powerupDuration;
    [SerializeField] private bool isPermanent;

    private HealthPowerup powerup;

    private void Start()
    {
        powerup = new HealthPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(healthBarRestore);
        powerup.SetPowerupDuration(powerupDuration);
        powerup.SetIsPermanent(isPermanent);
    }

    public override bool PrimaryAction(PowerupManager powerupManager)
    {
        if (powerupManager == null)
        {
            return false;
        }

        // Checks if object is not already at max health
        if (!powerupManager.GetComponent<Health>().IsCurrentValueAtMaxValue())
        {
            // Adds the powerup to the manager for its effect to be applied
            powerupManager.Add(powerup);
            return true;
        }
        else
        {
            return false;
        }
    }
}