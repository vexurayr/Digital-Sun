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

    public void Die()
    {
        Destroy(gameObject);
    }
}