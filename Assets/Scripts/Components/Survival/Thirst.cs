using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thirst : BaseValues
{
    #region MonoBehaviours
    protected override void Start()
    {
        base.Start();

        isDrainingValueOverTime = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isDebugging)
        {
            Debug.Log("Current Thirst: " + currentValue);
        }

        if (currentValue <= 0)
        {
            GetComponent<Health>().DecCurrentValueOverTime();
        }
    }

    #endregion MonoBehaviours

    #region OverrideFunctions
    // There will be a moment after drinking where the thirst bar won't drain
    public override void IncCurrentValue(float hungerRestored)
    {
        StopCoroutine("DrainValueDelay");

        isDrainingValueOverTime = false;

        base.IncCurrentValue(hungerRestored);

        StartCoroutine("DrainValueDelay");
    }

    public override void IncCurrentValueOverTime()
    {
        StopCoroutine("DrainValueDelay");

        isDrainingValueOverTime = false;

        base.IncCurrentValueOverTime();

        StartCoroutine("DrainValueDelay");
    }

    #endregion OverrideFunctions
}