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
    [SerializeField] private bool isDebugging;

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

    #region CheckTemperature
    // Still need to implement detection for player clothing
    public void CheckTemperature()
    {
        float hotProtection = GetComponent<Defense>().GetCurrentHotProtection();
        float coldProtection = GetComponent<Defense>().GetCurrentColdProtection();
        float currentTemp = EcoManager.instance.GetCurrentTemperature();

        // Outside temp is fine
        if (currentTemp < (stage1HotTemp + hotProtection) && currentTemp > (stage1ColdTemp - coldProtection))
        {
            return;
        }

        // Outside temp is too hot
        if (currentTemp >= (stage1HotTemp + hotProtection))
        {
            if (currentTemp >= (stage2HotTemp + hotProtection))
            {
                if (isDebugging)
                {
                    Debug.Log("Way too hot! Reached stage 2 hot temperature.");
                }

                GetComponent<Health>().DecCurrentValue(stage2DrainMult * Time.deltaTime);
            }
            else
            {
                if (isDebugging)
                {
                    Debug.Log("Too hot! Reached stage 1 hot temperature.");
                }

                GetComponent<Health>().DecCurrentValue(stage1DrainMult * Time.deltaTime);
            }
        }
        // Outside temp is too cold
        else if (currentTemp <= (stage1ColdTemp - coldProtection))
        {
            if (currentTemp <= (stage2ColdTemp - coldProtection))
            {
                if (isDebugging)
                {
                    Debug.Log("Way too cold! Reached stage 2 cold temperature.");
                }

                GetComponent<Health>().DecCurrentValue(stage2DrainMult * Time.deltaTime);
            }
            else
            {
                if (isDebugging)
                {
                    Debug.Log("Too cold! Reached stage 1 cold temperature.");
                }

                GetComponent<Health>().DecCurrentValue(stage1DrainMult * Time.deltaTime);
            }
        }
    }

    #endregion CheckTemperature
}