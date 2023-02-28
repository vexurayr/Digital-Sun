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
        bool canApply = true;
        int indexPowerupExists = 0;
        int index = 0;

        // See if the incoming powerup is already in the powerup manager's list
        foreach (Powerup power in powerups)
        {
            if (powerup.GetPowerupType() == power.GetPowerupType())
            {
                canApply = false;
                indexPowerupExists = index;
            }
            index++;
        }

        // Apply powerup if it's not already in this list
        if (canApply)
        {
            powerup.Apply(this);
            // Add powerup to list
            powerups.Add(powerup);
        }
        // If it is in the list, reset the time it lasts for
        else
        {
            Debug.Log("Remaining Duration: " + powerups[indexPowerupExists].GetPowerupDuration() +
                "\nResetting Duration To: " + powerup.GetPowerupDuration());
            powerups[indexPowerupExists].SetPowerupDuration(powerup.GetPowerupDuration());
        }
    }

    // This method will remove a powerup's effects from the object
    public void Remove(Powerup powerup)
    {
        // Remove the powerup from the object
        powerup.Remove(this);

        // Add powerup to list of powerups that will be removed after the foreach loop is done
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
                powerup.SetPowerupDuration(powerup.GetPowerupDuration() - Time.deltaTime);
                
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