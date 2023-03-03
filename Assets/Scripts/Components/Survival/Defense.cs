using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : BaseValues
{
    #region Variables
    private float currentHotProtection = 0;
    private float currentColdProtection = 0;

    #endregion Variables

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
            Debug.Log("Current Defense: " + currentValue +
                "\nCurrent Hot Protection: " + currentHotProtection +
                "\nCurrent Cold Protection: " + currentColdProtection);
        }
    }

    #endregion MonoBehaviours

    #region GetSet
    public float GetCurrentHotProtection()
    {
        return currentHotProtection;
    }

    public void SetCurrentHotProtection(float newAmount)
    {
        currentHotProtection = newAmount;
    }

    public float GetCurrentColdProtection()
    {
        return currentColdProtection;
    }

    public void SetCurrentColdProtection(float newAmount)
    {
        currentColdProtection = newAmount;
    }

    #endregion GetSet

    #region IncreaseDecrease
    public void IncCurrentHotProtection(float incAmount)
    {
        currentHotProtection += incAmount;
    }

    public void DecCurrentHotProtection(float decAmount)
    {
        currentHotProtection -= decAmount;
    }

    public void IncCurrentColdProtection(float incAmount)
    {
        currentColdProtection += incAmount;
    }

    public void DecCurrentColdProtection(float decAmount)
    {
        currentColdProtection -= decAmount;
    }

    #endregion IncreaseDecrease
}