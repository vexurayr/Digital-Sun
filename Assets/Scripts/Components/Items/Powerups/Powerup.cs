using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    #region Variables
    public enum PrimaryEffect
    {
        None,
        Health,
        Hunger,
        Thirst,
        Stamina,
        StaminaRegen
    }

    public enum SecondEffect
    {
        None,
        Health,
        Hunger,
        Thirst,
        Stamina,
        StaminaRegen
    }

    [Tooltip("Which survival system should the consumable effect?")][SerializeField] private PrimaryEffect primaryEffect;
    [Tooltip("Set to None if the consumable won't have a second effect.")][SerializeField] private SecondEffect secondEffect;
    [Tooltip("Being a flat amount versus a percent changes what this number means.")][SerializeField] private float primaryStatChangeAmount;
    [Tooltip("Being a flat amount versus a percent changes what this number means.")][SerializeField] private float secondStatChangeAmount;
    [Tooltip("Leave duration at 0 if the effect is instant.")][SerializeField] private float maxPowerupDuration;
    [Tooltip("If true, the effects will stack on top of their duration being reset.")][SerializeField] private bool isStackable;
    [Tooltip("If true, the effect will never wear off.")][SerializeField] private bool isPrimaryEffectPermanent;
    [Tooltip("If true, the effect will never wear off.")][SerializeField] private bool isSecondEffectPermanent;
    [Tooltip("If it's not a percent change, the stat change is a flat amount.")][SerializeField] private bool isPrimaryStatPercentChange;
    [Tooltip("If it's not a percent change, the stat change is a flat amount.")][SerializeField] private bool isSecondStatPercentChange;
    [Tooltip("Force primary effect to always apply to the target.")][SerializeField] private bool isPrimaryEffectAlwaysApplied;
    [Tooltip("Force second effect to always apply to the target.")][SerializeField] private bool isSecondEffectAlwaysApplied;
    // Will track stacking effects so Remove() is called for each stack applied
    private int timesPrimaryEffectApplied = 0;
    private int timesSecondEffectApplied = 0;
    private float currentPowerupDuration = 0f;

    #endregion Variables

    #region ApplyPrimaryEffect
    // This method will be called when the item is used
    public void ApplyPrimaryEffect(PowerupManager target)
    {
        currentPowerupDuration = maxPowerupDuration;

        if (primaryEffect == PrimaryEffect.None)
        {
            // Do nothing
        }
        else if (primaryEffect == PrimaryEffect.Health)
        {
            // Gets Health Component
            Health health = target.GetComponent<Health>();

            // Only heals if it has a health component
            if (health == null)
            {
                return;
            }

            // Changes healing amount if it's a percentage healing or flat healing
            if (isPrimaryStatPercentChange)
            {
                float maxHealth = health.GetMaxValue();

                health.IncCurrentValue(primaryStatChangeAmount * maxHealth);
            }
            else
            {
                health.IncCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Hunger)
        {
            Hunger hunger = target.GetComponent<Hunger>();

            if (hunger == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxHunger = hunger.GetMaxValue();

                hunger.IncCurrentValue(primaryStatChangeAmount * maxHunger);
            }
            else
            {
                hunger.IncCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Thirst)
        {
            Thirst thirst = target.GetComponent<Thirst>();

            if (thirst == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxThirst = thirst.GetMaxValue();

                thirst.IncCurrentValue(primaryStatChangeAmount * maxThirst);
            }
            else
            {
                thirst.IncCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Stamina)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxStamina = stamina.GetMaxValue();

                stamina.IncCurrentValue(primaryStatChangeAmount * maxStamina);
            }
            else
            {
                stamina.IncCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.StaminaRegen)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float regainMult = stamina.GetRegainMult();

                stamina.IncRegainMult(primaryStatChangeAmount * regainMult);
            }
            else
            {
                stamina.IncRegainMult(primaryStatChangeAmount);
            }
        }
    }

    #endregion ApplyPrimaryEffect

    #region ApplySecondEffect
    public void ApplySecondEffect(PowerupManager target)
    {
        currentPowerupDuration = maxPowerupDuration;

        if (secondEffect == SecondEffect.None)
        {
            // Do nothing
        }
        else if (secondEffect == SecondEffect.Health)
        {
            // Gets Health Component
            Health health = target.GetComponent<Health>();

            // Only heals if it has a health component
            if (health == null)
            {
                return;
            }

            // Changes healing amount if it's a percentage healing or flat healing
            if (isSecondStatPercentChange)
            {
                float maxHealth = health.GetMaxValue();

                health.IncCurrentValue(secondStatChangeAmount * maxHealth);
            }
            else
            {
                health.IncCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Hunger)
        {
            Hunger hunger = target.GetComponent<Hunger>();

            if (hunger == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxHunger = hunger.GetMaxValue();

                hunger.IncCurrentValue(secondStatChangeAmount * maxHunger);
            }
            else
            {
                hunger.IncCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Thirst)
        {
            Thirst thirst = target.GetComponent<Thirst>();

            if (thirst == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxThirst = thirst.GetMaxValue();

                thirst.IncCurrentValue(secondStatChangeAmount * maxThirst);
            }
            else
            {
                thirst.IncCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Stamina)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxStamina = stamina.GetMaxValue();

                stamina.IncCurrentValue(secondStatChangeAmount * maxStamina);
            }
            else
            {
                stamina.IncCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.StaminaRegen)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float regainMult = stamina.GetRegainMult();

                stamina.IncRegainMult(secondStatChangeAmount * regainMult);
            }
            else
            {
                stamina.IncRegainMult(secondStatChangeAmount);
            }
        }
    }

    #endregion ApplySecondEffect

    #region RemovePrimaryEffect
    // This method will be called when the item's effect wears off and it wasn't permanent
    public void RemovePrimaryEffect(PowerupManager target)
    {
        if (primaryEffect == PrimaryEffect.None)
        {
            // Do nothing
        }
        else if (primaryEffect == PrimaryEffect.Health)
        {
            // Gets Health Component
            Health health = target.GetComponent<Health>();

            // Only heals if it has a health component
            if (health == null)
            {
                return;
            }

            // Changes healing amount if it's a percentage healing or flat healing
            if (isPrimaryStatPercentChange)
            {
                float maxHealth = health.GetMaxValue();

                health.DecCurrentValue(primaryStatChangeAmount * maxHealth);
            }
            else
            {
                health.DecCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Hunger)
        {
            Hunger hunger = target.GetComponent<Hunger>();

            if (hunger == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxHunger = hunger.GetMaxValue();

                hunger.DecCurrentValue(primaryStatChangeAmount * maxHunger);
            }
            else
            {
                hunger.DecCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Thirst)
        {
            Thirst thirst = target.GetComponent<Thirst>();

            if (thirst == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxThirst = thirst.GetMaxValue();

                thirst.DecCurrentValue(primaryStatChangeAmount * maxThirst);
            }
            else
            {
                thirst.DecCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.Stamina)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float maxStamina = stamina.GetMaxValue();

                stamina.DecCurrentValue(primaryStatChangeAmount * maxStamina);
            }
            else
            {
                stamina.DecCurrentValue(primaryStatChangeAmount);
            }
        }
        else if (primaryEffect == PrimaryEffect.StaminaRegen)
        {
            Stamina stamina = target.GetComponent<Stamina>();
            
            if (stamina == null)
            {
                return;
            }

            if (isPrimaryStatPercentChange)
            {
                float regainMult = stamina.GetRegainMult();

                stamina.DecRegainMult(primaryStatChangeAmount * regainMult);
            }
            else
            {
                stamina.DecRegainMult(primaryStatChangeAmount);
            }
        }
    }

    #endregion RemovePrimaryEffect

    #region RemoveSecondEffect
    public void RemoveSecondEffect(PowerupManager target)
    {
        if (secondEffect == SecondEffect.None)
        {
            // Do nothing
        }
        else if (secondEffect == SecondEffect.Health)
        {
            // Gets Health Component
            Health health = target.GetComponent<Health>();

            // Only heals if it has a health component
            if (health == null)
            {
                return;
            }

            // Changes healing amount if it's a percentage healing or flat healing
            if (isSecondStatPercentChange)
            {
                float maxHealth = health.GetMaxValue();

                health.DecCurrentValue(secondStatChangeAmount * maxHealth);
            }
            else
            {
                health.DecCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Hunger)
        {
            Hunger hunger = target.GetComponent<Hunger>();

            if (hunger == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxHunger = hunger.GetMaxValue();

                hunger.DecCurrentValue(secondStatChangeAmount * maxHunger);
            }
            else
            {
                hunger.DecCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Thirst)
        {
            Thirst thirst = target.GetComponent<Thirst>();

            if (thirst == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxThirst = thirst.GetMaxValue();

                thirst.DecCurrentValue(secondStatChangeAmount * maxThirst);
            }
            else
            {
                thirst.DecCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.Stamina)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float maxStamina = stamina.GetMaxValue();

                stamina.DecCurrentValue(secondStatChangeAmount * maxStamina);
            }
            else
            {
                stamina.DecCurrentValue(secondStatChangeAmount);
            }
        }
        else if (secondEffect == SecondEffect.StaminaRegen)
        {
            Stamina stamina = target.GetComponent<Stamina>();

            if (stamina == null)
            {
                return;
            }

            if (isSecondStatPercentChange)
            {
                float regainMult = stamina.GetRegainMult();

                stamina.DecRegainMult(secondStatChangeAmount * regainMult);
            }
            else
            {
                stamina.DecRegainMult(secondStatChangeAmount);
            }
        }
    }

    #endregion RemoveSecondEffect

    #region GetSet
    public float GetPrimaryStatChangeAmount()
    {
        return primaryStatChangeAmount;
    }

    public void SetPrimaryStatChangeAmount(float newAmount)
    {
        if (newAmount <= 0)
        {
            // Do Nothing
        }
        else
        {
            primaryStatChangeAmount = newAmount;
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

    public float GetMaxPowerupDuration()
    {
        return maxPowerupDuration;
    }

    public void SetMaxPowerupDuration(float newDuration)
    {
        maxPowerupDuration = newDuration;
    }

    public float GetCurrentPowerupDuration()
    {
        return currentPowerupDuration;
    }

    public void SetCurrentPowerupDuration(float newDuration)
    {
        currentPowerupDuration = newDuration;
    }

    public bool GetIsStackable()
    {
        return isStackable;
    }

    public void SetIsStackable(bool newState)
    {
        isStackable = newState;
    }

    public bool GetIsPrimaryPermanent()
    {
        return isPrimaryEffectPermanent;
    }

    public void SetIsPrimaryPermanent(bool newState)
    {
        isPrimaryEffectPermanent = newState;
    }

    public bool GetIsSecondPermanent()
    {
        return isSecondEffectPermanent;
    }

    public void SetIsSecondPermanent(bool newState)
    {
        isSecondEffectPermanent = newState;
    }

    public PrimaryEffect GetPrimaryEffect()
    {
        return primaryEffect;
    }

    public void SetPrimaryEffect(PrimaryEffect newEffect)
    {
        primaryEffect = newEffect;
    }

    public SecondEffect GetSecondEffect()
    {
        return secondEffect;
    }

    public void SetSecondEffect(SecondEffect newEffect)
    {
        secondEffect = newEffect;
    }

    public bool GetIsPrimaryPercentChange()
    {
        return isPrimaryStatPercentChange;
    }

    public void SetIsPrimaryPercentChange(bool newState)
    {
        isPrimaryStatPercentChange = newState;
    }

    public bool GetIsSecondPercentChange()
    {
        return isSecondStatPercentChange;
    }

    public void SetIsSecondPercentChange(bool newState)
    {
        isSecondStatPercentChange = newState;
    }

    public bool GetIsPrimaryEffectAlwaysApplied()
    {
        return isPrimaryEffectAlwaysApplied;
    }

    public void SetIsPrimaryEffectAlwaysApplied(bool newState)
    {
        isPrimaryEffectAlwaysApplied = newState;
    }

    public bool GetIsSecondEffectAlwaysApplied()
    {
        return isSecondEffectAlwaysApplied;
    }

    public void SetIsSecondEffectAlwaysApplied(bool newState)
    {
        isSecondEffectAlwaysApplied = newState;
    }

    public int GetTimesPrimaryEffectApplied()
    {
        return timesPrimaryEffectApplied;
    }

    public void SetTimesPrimaryEffectApplied(int applied)
    {
        timesPrimaryEffectApplied = applied;
    }

    public int GetTimesSecondEffectApplied()
    {
        return timesSecondEffectApplied;
    }

    public void SetTimesSecondEffectApplied(int applied)
    {
        timesSecondEffectApplied = applied;
    }

    #endregion GetSet
}