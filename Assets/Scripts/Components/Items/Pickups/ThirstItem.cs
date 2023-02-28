using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class ThirstItem : InventoryItem
{
    [SerializeField] private PowerupType powerupType;
    [SerializeField] protected float thirstBarRestore;
    [SerializeField] private float powerupDuration;
    [SerializeField] private bool isPermanent;
    
    private ThirstPowerup powerup;

    private void Start()
    {
        powerup = new ThirstPowerup();
        powerup.SetPowerupType(powerupType);
        powerup.SetStatChangeAmount(thirstBarRestore);
        powerup.SetPowerupDuration(powerupDuration);
        powerup.SetIsPermanent(isPermanent);
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