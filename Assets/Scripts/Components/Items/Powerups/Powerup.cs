using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Powerup
{
    public enum PowerupType
    {
        Health,
        Food,
        Thirst,
        Stamina
    }

    protected PowerupType powerupType;
    protected float statChangeAmount;
    protected float secondStatChangeAmount;
    protected float powerupDuration;
    protected bool isPermanent;

    // This method will be called when the item is picked up
    public virtual void Apply(PowerupManager target)
    {}

    // This method will be called when the item's effect wears off
    public virtual void Remove(PowerupManager target)
    {}

    public float GetStatChangeAmount()
    {
        return statChangeAmount;
    }

    public void SetStatChangeAmount(float newAmount)
    {
        if (newAmount <= 0)
        {
            // Do Nothing
        }
        else
        {
            statChangeAmount = newAmount;
        }
    }

    public float GetSecondStatChangeAmount()
    {
        return secondStatChangeAmount;
    }

    public void SetSecondStatChangeAmount(float newAmount)
    {
        secondStatChangeAmount = newAmount;
    }

    public float GetPowerupDuration()
    {
        return powerupDuration;
    }

    public void SetPowerupDuration(float newDuration)
    {
        powerupDuration = newDuration;
    }

    public bool GetIsPermanent()
    {
        return isPermanent;
    }

    public void SetIsPermanent(bool newState)
    {
        isPermanent = newState;
    }

    public PowerupType GetPowerupType()
    {
        return powerupType;
    }

    public void SetPowerupType(PowerupType newType)
    {
        powerupType = newType;
    }
}