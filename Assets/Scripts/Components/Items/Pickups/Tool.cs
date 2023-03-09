using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class Tool : InventoryItem
{
    #region Variables
    [SerializeField] private float damageToEnemy;
    [SerializeField] private float damageToSpecialty;
    [SerializeField] private float animationSpeed;
    [SerializeField] private Transform transformInHand;

    private Collider hitbox;

    #endregion Variables

    #region MonoBehaviours
    private void Start()
    {
        hitbox = GetComponent<Collider>();
    }

    #endregion MonoBehaviours

    #region PrimaryAction
    public override bool PrimaryAction()
    {


        return false;
    }

    #endregion PrimaryAction
}