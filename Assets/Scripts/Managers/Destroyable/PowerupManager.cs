using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will go on any object capable of using powerups
public class PowerupManager : MonoBehaviour
{
    // All active powerups in powerup manager
    private List<Powerup> powerups;
    private List<Powerup> removedPowerupQueue;

    private void Start()
    {
        // Makes list empty, not null
        powerups = new List<Powerup>();
        removedPowerupQueue = new List<Powerup>();
    }

    private void Update()
    {
        if (powerups.Count > 0)
        {
            DecrementPowerupTimers();
        }
    }

    private void LateUpdate()
    {
        if (removedPowerupQueue.Count > 0)
        {
            ApplyRemovePowerupsQueue();
        }
    }

    // This method activates the powerup's effects and adds it to a list of active powerups
    public void Add(Powerup powerup)
    {
        Debug.Log("Incoming powerup: " + powerup.GetPowerupDuration());
        bool isAlreadyApplied = false;
        int indexPowerupExists = 0;
        int index = 0;

        // See if the incoming powerup is already in the powerup manager's list
        foreach (Powerup power in powerups)
        {
            if (powerup.GetPowerupType() == power.GetPowerupType())
            {
                isAlreadyApplied = true;
                indexPowerupExists = index;
            }
            index++;
        }

        // Apply powerup if it's not already in this list
        if (!isAlreadyApplied)
        {
            powerup.ApplyPrimaryEffect(this);
            powerup.ApplySecondEffect(this);
            
            powerup.SetTimesApplied(powerup.GetTimesApplied() + 1);
            // Add powerup to list
            powerups.Add(powerup);
        }
        // If it is in the list and can't stack, just reset its timer
        else if (!powerup.GetIsStackable())
        {
            powerups[indexPowerupExists].SetPowerupDuration(powerup.GetPowerupDuration());

            if (powerup.GetIsPrimaryEffectAlwaysApplied())
            {
                powerup.ApplyPrimaryEffect(this);
                powerups[indexPowerupExists].SetTimesApplied(powerup.GetTimesApplied() + 1);
            }
            if (powerup.GetIsSecondEffectAlwaysApplied())
            {
                powerup.ApplySecondEffect(this);
            }
        }
        // It is in the list and can stack, also reset the timer
        else
        {
            powerup.ApplyPrimaryEffect(this);
            powerup.ApplySecondEffect(this);

            powerups[indexPowerupExists].SetTimesApplied(powerup.GetTimesApplied() + 1);
            powerups[indexPowerupExists].SetPowerupDuration(powerup.GetPowerupDuration());
        }
    }

    // This method will remove a powerup's effects from the object
    public void Remove(Powerup powerup)
    {
        // For each time the effect was applied, remove its stat change
        for (int i = powerup.GetTimesApplied(); i <= 0; i--)
        {
            powerup.RemovePrimaryEffect(this);
            powerup.RemoveSecondEffect(this);
        }

        // Add powerup to list of powerups that will be removed after DecrementPowerupTimers() foreach is done
        removedPowerupQueue.Add(powerup);
    }

    public void DecrementPowerupTimers()
    {
        foreach (Powerup powerup in powerups)
        {
            // Can't decrease timer of permanent powerup
            if (!powerup.GetIsPermanent())
            {
                // Decrease the time it has remaining
                float remainingDuration = powerup.GetPowerupDuration();
                remainingDuration -= Time.deltaTime;
                powerup.SetPowerupDuration(remainingDuration);
                
                // Remove powerup if time is up
                if (powerup.GetPowerupDuration() <= 0)
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        foreach (Powerup powerup in removedPowerupQueue)
        {
            powerups.Remove(powerup);
        }
        // Clear list for next time powerups need to be removed
        removedPowerupQueue.Clear();
    }
}