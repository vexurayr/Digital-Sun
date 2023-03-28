using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class Consumable : InventoryItem
{
    #region Variables
    
    [Tooltip("Create a new powerup prefab and provide it here.")] [SerializeField] private Powerup powerup;

    #endregion Variables

    #region PrimaryAction
    public override bool PrimaryAction(GameObject player)
    {
        if (powerup.GetPrimaryEffect() == PrimaryEffect.None)
        {
            // Do Nothing
            return false;
        }
        else if (powerup.GetPrimaryEffect() == PrimaryEffect.Health)
        {
            // Checks if object is not already at max health
            if (!player.GetComponent<Health>().IsCurrentValueAtMaxValue())
            {
                // Adds the powerup to the manager for its effect to be applied
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (powerup.GetPrimaryEffect() == PrimaryEffect.Hunger)
        {
            if (!player.GetComponent<Hunger>().IsCurrentValueAtMaxValue())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (powerup.GetPrimaryEffect() == PrimaryEffect.Thirst)
        {
            // Checks if object is not already at max health
            if (!player.GetComponent<Thirst>().IsCurrentValueAtMaxValue())
            {
                // Adds the powerup to the manager for its effect to be applied
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (powerup.GetPrimaryEffect() == PrimaryEffect.Stamina)
        {
            if (!player.GetComponent<Stamina>().IsCurrentValueAtMaxValue())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
            {
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (powerup.GetPrimaryEffect() == PrimaryEffect.StaminaRegen)
        {
            player.GetComponent<PowerupManager>().Add(powerup);
            return true;
        }

        // Will only make it here if something goes wrong with the enum
        return false;
    }

    #endregion PrimaryAction
}