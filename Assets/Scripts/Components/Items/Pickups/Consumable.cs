using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class Consumable : InventoryItem
{
    #region Variables
    [Tooltip("Which survival system should the consumable effect?")] [SerializeField] private PrimaryEffect primaryEffect;
    [Tooltip("Set to None if the consumable won't have a second effect.")] [SerializeField] private SecondEffect secondEffect;
    [Tooltip("Being a flat amount versus a percent changes what this number means.")] [SerializeField] private float primaryStatChangeAmount;
    [Tooltip("Being a flat amount versus a percent changes what this number means.")] [SerializeField] private float secondStatChangeAmount;
    [Tooltip("Leave duration at 0 if the effect is instant.")] [SerializeField] private float maxPowerupDuration;
    [Tooltip("If true, the effects will stack on top of their duration being reset.")] [SerializeField] private bool isStackable;
    [Tooltip("If true, the effect will never wear off.")] [SerializeField] private bool isPrimaryEffectPermanent;
    [Tooltip("If true, the effect will never wear off.")] [SerializeField] private bool isSecondEffectPermanent;
    [Tooltip("If it's not a percent change, the stat change is a flat amount.")] [SerializeField] private bool isPrimaryStatPercentChange;
    [Tooltip("If it's not a percent change, the stat change is a flat amount.")] [SerializeField] private bool isSecondStatPercentChange;
    [Tooltip("Force primary effect to always apply to the target.")] [SerializeField] private bool isPrimaryEffectAlwaysApplied;
    [Tooltip("Force second effect to always apply to the target.")] [SerializeField] private bool isSecondEffectAlwaysApplied;

    private Powerup powerup;

    #endregion Variables

    #region MonoBehaviours
    private void Start()
    {
        powerup = new Powerup();
        powerup.SetPrimaryEffect(primaryEffect);
        powerup.SetSecondEffect(secondEffect);
        powerup.SetPrimaryStatChangeAmount(primaryStatChangeAmount);
        powerup.SetSecondStatChangeAmount(secondStatChangeAmount);
        powerup.SetMaxPowerupDuration(maxPowerupDuration);
        powerup.SetCurrentPowerupDuration(maxPowerupDuration);
        powerup.SetIsStackable(isStackable);
        powerup.SetIsPrimaryPermanent(isPrimaryEffectPermanent);
        powerup.SetIsSecondPermanent(isSecondEffectPermanent);
        powerup.SetIsPrimaryPercentChange(isPrimaryStatPercentChange);
        powerup.SetIsSecondPercentChange(isSecondStatPercentChange);
        powerup.SetIsPrimaryEffectAlwaysApplied(isPrimaryEffectAlwaysApplied);
        powerup.SetIsSecondEffectAlwaysApplied(isSecondEffectAlwaysApplied);
    }

    #endregion MonoBehaviours

    #region PrimaryAction
    public override bool PrimaryAction(GameObject player)
    {
        if (primaryEffect == PrimaryEffect.None)
        {
            // Do Nothing
            return false;
        }
        else if (primaryEffect == PrimaryEffect.Health)
        {
            // Checks if object is not already at max health
            if (!player.GetComponent<Health>().IsCurrentValueAtMaxValue())
            {
                // Adds the powerup to the manager for its effect to be applied
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (isPrimaryEffectAlwaysApplied || isSecondEffectAlwaysApplied)
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (primaryEffect == PrimaryEffect.Hunger)
        {
            if (!player.GetComponent<Hunger>().IsCurrentValueAtMaxValue())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (isPrimaryEffectAlwaysApplied || isSecondEffectAlwaysApplied)
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (primaryEffect == PrimaryEffect.Thirst)
        {
            // Checks if object is not already at max health
            if (!player.GetComponent<Thirst>().IsCurrentValueAtMaxValue())
            {
                // Adds the powerup to the manager for its effect to be applied
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (isPrimaryEffectAlwaysApplied || isSecondEffectAlwaysApplied)
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (primaryEffect == PrimaryEffect.Stamina)
        {
            if (!player.GetComponent<Stamina>().IsCurrentValueAtMaxValue())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (isPrimaryEffectAlwaysApplied || isSecondEffectAlwaysApplied)
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (primaryEffect == PrimaryEffect.StaminaRegen)
        {
            player.GetComponent<PowerupManager>().Add(powerup);
            return true;
        }

        // Will only make it here if something goes wrong with the enum
        return false;
    }

    #endregion PrimaryAction
}