using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem
{
    #region Variables
    [SerializeField] private float damageToEnemy;
    [SerializeField] private string animationTriggerName;
    [SerializeField] private float animationSpeed;
    [SerializeField] private float transformInHandX, transformInHandY, transformInHandZ;
    [SerializeField] private float rotationInHandX, rotationInHandY, rotationInHandZ;
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
    public override bool PrimaryAction()
    {
        // If the animation is ready to play
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            animator.SetTrigger(animationTriggerName);
            
            return true;
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

    public override Vector3 GetTransformInHand()
    {
        return new Vector3(transformInHandX, transformInHandY, transformInHandZ);
    }

    public override Vector3 GetRotationInHand()
    {
        return new Vector3(rotationInHandX, rotationInHandY, rotationInHandZ);
    }
}