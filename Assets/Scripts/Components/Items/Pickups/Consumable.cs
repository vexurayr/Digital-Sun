using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class Consumable : InventoryItem
{
    #region Variables
    
    [Tooltip("Create a new powerup prefab and provide it here.")] [SerializeField] private Powerup powerup;
    [SerializeField] private string animationPrimaryTriggerName;
    [SerializeField] private float animationSpeed;

    private Animator animator;

    #endregion Variables

    #region MonoBehaviours
    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.speed = animationSpeed;
    }

    #endregion MonoBehaviours

    #region PrimaryAction
    public override bool PrimaryAction(GameObject player, InventoryItem emptyItem)
    {
        if (powerup.GetPrimaryEffect() == PrimaryEffect.None)
        {
            // Do Nothing
            return false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if (powerup.GetPrimaryEffect() == PrimaryEffect.Health)
            {
                // Checks if object is not already at max health
                if (!player.GetComponent<Health>().IsCurrentValueAtMaxValue())
                {
                    animator.SetTrigger(animationPrimaryTriggerName);
                    // Adds the powerup to the manager for its effect to be applied
                    player.GetComponent<PowerupManager>().Add(powerup);
                    return true;
                }
                else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
                {
                    animator.SetTrigger(animationPrimaryTriggerName);
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
                    animator.SetTrigger(animationPrimaryTriggerName);
                    player.GetComponent<PowerupManager>().Add(powerup);
                    return true;
                }
                else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
                {
                    animator.SetTrigger(animationPrimaryTriggerName);
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
                    animator.SetTrigger(animationPrimaryTriggerName);
                    // Adds the powerup to the manager for its effect to be applied
                    player.GetComponent<PowerupManager>().Add(powerup);
                    return true;
                }
                else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
                {
                    animator.SetTrigger(animationPrimaryTriggerName);
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
                    animator.SetTrigger(animationPrimaryTriggerName);
                    player.GetComponent<PowerupManager>().Add(powerup);
                    return true;
                }
                else if (powerup.GetIsPrimaryEffectAlwaysApplied() || powerup.GetIsSecondEffectAlwaysApplied())
                {
                    animator.SetTrigger(animationPrimaryTriggerName);
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
                animator.SetTrigger(animationPrimaryTriggerName);
                player.GetComponent<PowerupManager>().Add(powerup);
                return true;
            }
        }

        // Will only make it here if something goes wrong with the enum
        return false;
    }

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