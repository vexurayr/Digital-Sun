using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour
{
    [SerializeField] private float stage1HotTemp;
    [SerializeField] private float stage2HotTemp;
    [SerializeField] private float stage1ColdTemp;
    [SerializeField] private float stage2ColdTemp;
    [SerializeField] private float stage1DrainMult;
    [SerializeField] private float stage2DrainMult;

    private void Update()
    {
        CheckTemperature();
    }

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
}