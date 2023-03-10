using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Health : BaseValues
{
    #region MonoBehaviours
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isDebugging)
        {
            Debug.Log("Current Health: " + currentValue);
        }
    }

    #endregion MonoBehaviours

    #region OverrideFunctions
    public override void DecCurrentValue(float damage)
    {
        base.DecCurrentValue(damage);

        if (currentValue <= 0)
        {
            Die();
        }
    }

    public override void DecCurrentValueOverTime()
    {
        base.DecCurrentValueOverTime();

        if (currentValue <= 0)
        {
            Die();
        }
    }

    #endregion OverrideFunctions

    public void OnCollisionEnter(Collision collision)
    {
        // Check if a weapon's hitbox has collided with the pawn
        if (collision.gameObject.GetComponent<DamageSource>())
        {
            Weapon weapon = collision.gameObject.GetComponentInParent<Weapon>();
            Defense defense = this.gameObject.GetComponent<Defense>();
            float damageTaken = weapon.GetDamageToEnemy();
            float totalDamage = damageTaken - (damageTaken * defense.GetCurrentValue());

            DecCurrentValue(totalDamage);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}