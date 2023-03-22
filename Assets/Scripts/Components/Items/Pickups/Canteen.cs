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

    // This function will drink aka lose a charge from a hand slot
    public override bool PrimaryAction(GameObject player, InventoryItem otherCanteen)
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
                animator.SetTrigger(animationPrimaryTriggerName);

                thirst.IncCurrentValue(thirstRestored);

                chargesStored--;

                otherCanteen.SetChargesStored(chargesStored);

                return true;
            }
        }

        return false;
    }

    // This function will drink aka lose a charge from an inventory slot
    public override bool PrimaryAction(GameObject player)
    {
        if (!player.GetComponent<Thirst>() || chargesStored <= 0)
        {
            return false;
        }

        Thirst thirst = player.GetComponent<Thirst>();

        thirst.IncCurrentValue(thirstRestored);

        chargesStored--;

        return true;
    }

    // This function will fill the canteen aka gain all charges
    public override bool SecondaryAction(GameObject player, InventoryItem otherCanteen)
    {
        if (chargesStored >= maxCharges)
        {
            return false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            animator.SetTrigger(animationSecondaryTriggerName);

            chargesStored = maxCharges;

            otherCanteen.SetChargesStored(chargesStored);

            return true;
        }

        return false;
    }

    public override int GetChargesStored()
    {
        return chargesStored;
    }

    public override void SetChargesStored(int value)
    {
        chargesStored = value;
    }
}