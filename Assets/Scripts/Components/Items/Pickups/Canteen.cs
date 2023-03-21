using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canteen : InventoryItem
{
    [SerializeField] private int maxCharges;
    [SerializeField] private float thirstRestored;
    [SerializeField] private string animationPrimaryTriggerName;
    [SerializeField] private string animationSecondaryTriggerName;
    [SerializeField] private float animationSpeed;

    private int chargesStored = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.speed = animationSpeed;
    }

    // This function will drink aka lose a charge
    public override bool PrimaryAction(GameObject player)
    {
        if (!player.GetComponent<Thirst>() || chargesStored <= 0)
        {
            return false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            Thirst thirst = player.GetComponent<Thirst>();

            if (!thirst.IsCurrentValueAtMaxValue())
            {
                Debug.Log("Drinking water!");
                animator.SetTrigger(animationPrimaryTriggerName);
            }

            thirst.IncCurrentValue(thirstRestored);

            chargesStored--;

            return true;
        }

        return false;
    }

    // This function will fill the canteen aka gain all charges
    public override bool SecondaryAction()
    {
        if (chargesStored >= maxCharges)
        {
            return false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            Debug.Log("Canteen refilled!");
            animator.SetTrigger(animationSecondaryTriggerName);

            chargesStored = maxCharges;

            return true;
        }

        return false;
    }
}