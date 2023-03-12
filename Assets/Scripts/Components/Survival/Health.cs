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

    public virtual void OnTriggerEnter(Collider collider)
    {
        // Check if a weapon's hitbox has collided with the pawn
        if (collider.gameObject.GetComponent<DamageSource>())
        {
            Weapon weapon;
            Tool tool;
            Defense defense = this.gameObject.GetComponent<Defense>();
            float damageTaken = 0;
            
            if (collider.gameObject.GetComponentInParent<Weapon>())
            {
                weapon = collider.gameObject.GetComponentInParent<Weapon>();
                damageTaken = weapon.GetDamageToEnemy();
            }
            else if (collider.gameObject.GetComponentInParent<Tool>())
            {
                tool = collider.gameObject.GetComponentInParent<Tool>();
                damageTaken = tool.GetDamageToEnemy();
            }

            float totalDamage = damageTaken - (damageTaken * defense.GetCurrentValue());

            DecCurrentValue(totalDamage);
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}