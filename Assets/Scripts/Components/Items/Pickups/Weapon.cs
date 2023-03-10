using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem
{
    #region Variables
    [SerializeField] private float damageToEnemy;
    [SerializeField] private float staminaCost;
    [SerializeField] private string animationTriggerName;
    [SerializeField] private float animationSpeed;
    [SerializeField] private Collider hitbox;

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
    public override bool PrimaryAction(GameObject player)
    {
        // If the animation is ready to play
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if (player.GetComponent<Stamina>().GetCurrentValue() >= staminaCost)
            {
                animator.SetTrigger(animationTriggerName);

                player.GetComponent<Stamina>().DecCurrentValue(staminaCost);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    #endregion PrimaryAction

    public float GetDamageToEnemy()
    {
        return damageToEnemy;
    }
}