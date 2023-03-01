using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class StaminaItem : InventoryItem
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] private float staminaRegainRateIncrease;
    [SerializeField] private float thirstBarRestore;
    [SerializeField] private float powerupDuration;
    [SerializeField] private bool isStackable;
    [SerializeField] private bool isPermanent;
    [SerializeField] private bool isPrimaryEffectAlwaysApplied;
    [SerializeField] private bool isSecondEffectAlwaysApplied;

    private StaminaPowerup powerup;

    private void Start()
    {
        powerup = new StaminaPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(staminaRegainRateIncrease);
        powerup.SetSecondStatChangeAmount(thirstBarRestore);
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

        powerupManager.Add(powerup);
        return true;
    }
}