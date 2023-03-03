using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    #region Variables
    [SerializeField] private float stage1HotTemp;
    [SerializeField] private float stage2HotTemp;
    [SerializeField] private float stage1ColdTemp;
    [SerializeField] private float stage2ColdTemp;
    [SerializeField] private float stage1DrainMult;
    [SerializeField] private float stage2DrainMult;

    #endregion Variables

    #region MonoBehaviours
    private void Update()
    {
        CheckTemperature();
    }

    #endregion MonoBehaviours

    #region GetSet
    public float GetStage1HotTemp()
    {
        return stage1HotTemp;
    }

    public float GetStage2HotTemp()
    {
        return stage2HotTemp;
    }

    public float GetStage1ColdTemp()
    {
        return stage1ColdTemp;
    }

    public float GetStage2ColdTemp()
    {
        return stage2ColdTemp;
    }

    #endregion GetSet

    #region IncreaseDecrease
    public void IncHotTempStages(float incAmount)
    {
        stage1HotTemp += incAmount;
        stage2HotTemp += incAmount;
    }

    public void DecHotTempStages(float decAmount)
    {
        stage1HotTemp -= decAmount;
        stage2HotTemp -= decAmount;
    }

    public void IncColdTempStages(float incAmount)
    {
        stage1ColdTemp += incAmount;
        stage2ColdTemp += incAmount;
    }

    public void DecColdTempStages(float decAmount)
    {
        stage1ColdTemp -= decAmount;
        stage2ColdTemp -= decAmount;
    }

    #endregion IncreaseDecrease

    #region CheckTemperature
    // Still need to implement detection for player clothing
    public void CheckTemperature()
    {
        float currentTemp = EcoManager.instance.GetCurrentTemperature();

        // Outside temp is fine
        if (currentTemp < stage1HotTemp && currentTemp > stage1ColdTemp)
        {
            return;
        }

        // Outside temp is too hot
        if (currentTemp >= stage1HotTemp )
        {
            if (currentTemp >= stage2HotTemp)
            {
                GetComponent<Health>().DecCurrentValue(stage2DrainMult * Time.deltaTime);
            }
            else
            {
                GetComponent<Health>().DecCurrentValue(stage1DrainMult * Time.deltaTime);
            }
        }
        // Outside temp is too cold
        else if (currentTemp <= stage1ColdTemp)
        {
            if (currentTemp <= stage2HotTemp)
            {
                GetComponent<Health>().DecCurrentValue(stage2DrainMult * Time.deltaTime);
            }
            else
            {
                GetComponent<Health>().DecCurrentValue(stage1DrainMult * Time.deltaTime);
            }
        }
    }

    #endregion CheckTemperature
}