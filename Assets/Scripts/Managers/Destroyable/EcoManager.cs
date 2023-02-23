using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EcoManager : MonoBehaviour
{
    public static EcoManager instance { get; private set; }

    [SerializeField] private bool isStartingTimeRandom;
    [SerializeField] private float startingTimeOfDay;
    [SerializeField] private float maxTemperature;
    [SerializeField] private float minTemperature;
    [SerializeField] private bool isDebugging;

    private float currentTimeOfDay;
    private float currentTemperature;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (isStartingTimeRandom)
        {
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            currentTimeOfDay = UnityEngine.Random.Range(0f, 24f);
            currentTemperature = ConvertTimeToTemp();
        }
        else
        {
            currentTimeOfDay = startingTimeOfDay;
            currentTemperature = ConvertTimeToTemp();
        }
    }

    private void Update()
    {
        if (isDebugging)
        {
            Debug.Log("Current Time: " + currentTimeOfDay + "\nCurrent Temp: " + currentTemperature);
        }
    }

    public float GetCurrentTimeOfDay()
    {
        return currentTimeOfDay;
    }

    public float GetCurrentTemperature()
    {
        return currentTemperature;
    }

    public float ConvertTimeToTemp()
    {
        float timeRatioA = currentTimeOfDay / 12f;
        float timeRatioB = (currentTimeOfDay - 12) / 12f;
        float finalTimeRatio = timeRatioA;

        if (currentTimeOfDay > 12f)
        {
            finalTimeRatio = 1 - timeRatioB;
        }

        float tempRange = maxTemperature - minTemperature;
        float newRatio = tempRange * finalTimeRatio;

        float newTemp = newRatio + minTemperature;

        return newTemp;
    }
}