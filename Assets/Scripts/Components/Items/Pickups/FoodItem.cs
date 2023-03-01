using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class FoodItem : InventoryItem
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] private float hungerBarRestore;
    [SerializeField] private float powerupDuration;
    [SerializeField] private bool isStackable;
    [SerializeField] private bool isPermanent;
    [SerializeField] private bool isPrimaryEffectAlwaysApplied;
    [SerializeField] private bool isSecondEffectAlwaysApplied;

    private FoodPowerup powerup;
    
    private void Start()
    {
        powerup = new FoodPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(hungerBarRestore);
        powerup.SetPowerupDuration(powerupDuration);
        powerup.SetIsStackable(isStackable);
        powerup.SetIsPermanent(isPermanent);
        powerup.SetIsPrimaryEffectAlwaysApplied(isPrimaryEffectAlwaysApplied);
        powerup.SetIsSecondEffectAlwaysApplied(isSecondEffectAlwaysApplied);
    }

    public override bool PrimaryAction(PowerupManager powerupManager)
    {
        if (powerupManager == null)
        {
            return false;
        }

        // Checks if object is not already at max hunger
        if (!powerupManager.GetComponent<Hunger>().IsCurrentValueAtMaxValue())
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