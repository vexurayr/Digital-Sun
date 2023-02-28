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
    [SerializeField] private bool isPermanent;

    private StaminaPowerup powerup;

    private void Start()
    {
        powerup = new StaminaPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(staminaRegainRateIncrease);
        powerup.SetSecondStatChangeAmount(thirstBarRestore);
        powerup.SetPowerupDuration(powerupDuration);
        powerup.SetIsPermanent(isPermanent);
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