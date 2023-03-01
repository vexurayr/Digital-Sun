using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class ThirstItem : InventoryItem
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] private float thirstBarRestore;
    [SerializeField] private float powerupDuration;
    [SerializeField] private bool isStackable;
    [SerializeField] private bool isPermanent;
    [SerializeField] private bool isPrimaryEffectAlwaysApplied;
    [SerializeField] private bool isSecondEffectAlwaysApplied;
    
    private ThirstPowerup powerup;

    private void Start()
    {
        powerup = new ThirstPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(thirstBarRestore);
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

        if (!powerupManager.GetComponent<Thirst>().IsCurrentValueAtMaxValue())
        {
            powerupManager.Add(powerup);
            return true;
        }
        else
        {
            return false;
        }
    }
}