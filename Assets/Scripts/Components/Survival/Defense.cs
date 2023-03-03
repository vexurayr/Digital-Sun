using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : BaseValues
{
    #region Variables
    private float currentTemperatureProtection;

    #endregion Variables

    #region MonoBehaviours
    protected override void Start()
    {
        base.Start();

        currentTemperatureProtection = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (isDebugging)
        {
            Debug.Log("Current Defense: " + currentValue);
        }
    }

    #endregion MonoBehaviours

    #region GetSet
    public float GetCurrentTemperatureProtection()
    {
        return currentTemperatureProtection;
    }

    public void SetCurrentTemperatureProtection(float newAmount)
    {
        currentTemperatureProtection = newAmount;
    }

    #endregion GetSet

    #region IncreaseDecrease
    public void IncCurrentTemperatureProtection(float incAmount)
    {
        currentTemperatureProtection += incAmount;
    }

    public void DecCurrentTemperatureProtection(float decAmount)
    {
        currentTemperatureProtection -= decAmount;
    }

    #endregion HelperFunctions
}