using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : BaseValues
{
    #region MonoBehaviours
    protected override void Start()
    {
        base.Start();

        isRegainingValueOverTime = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isDebugging)
        {
            Debug.Log("Current Stamina: " + currentValue);
        }
    }

    #endregion MonoBehaviours

    #region OverrideFunctions
    // Used for player actions like chopping down trees
    public override void DecCurrentValue(float staminaSpent)
    {
        StopCoroutine("RegainValueDelay");

        isRegainingValueOverTime = false;

        base.DecCurrentValue(staminaSpent);

        StartCoroutine("RegainValueDelay");
    }

    public override void DecCurrentValueOverTime()
    {
        StopCoroutine("RegainValueDelay");

        isRegainingValueOverTime = false;

        base.DecCurrentValueOverTime();

        StartCoroutine("RegainValueDelay");
    }

    #endregion OverrideFunctions
}