using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EcoManager : MonoBehaviour
{
    public static EcoManager instance { get; private set; }

    [SerializeField] private GameObject sun;
    [SerializeField] private bool isStartingTimeRandom;
    [SerializeField] private bool isTimeChanging;
    [SerializeField] private float startingTimeOfDay;
    [SerializeField] private float maxTemperature;
    [SerializeField] private float minTemperature;
    [SerializeField] private bool isDebugging;

    private float sunRotationAtMidnight = 265;
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
        ChangeSunPosition();
    }

    private void Update()
    {
        if (isTimeChanging)
        {
            PassTime();
        }

        if (isDebugging)
        {
            Debug.Log("Current Time: " + currentTimeOfDay + "\nCurrent Temp: " + currentTemperature);
        }
    }

    public float GetCurrentHour()
    {
        return currentTimeOfDay;
    }

    public float GetCurrentMinute()
    {
        float decimalValue = currentTimeOfDay % 1;
        float minutes = decimalValue * 60;

        return minutes;
    }

    public float GetCurrentSecond()
    {
        float decimalValue = currentTimeOfDay % 1;
        float minutes = decimalValue * 60;
        float seconds = minutes * 60;
        seconds = seconds % 60;

        return seconds;
    }

    public float GetCurrentTemperature()
    {
        return currentTemperature;
    }

    public void PassTime()
    {
        currentTimeOfDay += (0.05f * Time.deltaTime);
        currentTimeOfDay = Mathf.Clamp(currentTimeOfDay, 0f, 24f);

        if (currentTimeOfDay >= 24)
        {
            currentTimeOfDay = 0;
        }

        ChangeSunPosition();
        currentTemperature = ConvertTimeToTemp();
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

    public void ChangeSunPosition()
    {
        sun.transform.rotation = Quaternion.Euler(ConvertTimeToSunPosition(), sun.transform.rotation.y, sun.transform.rotation.z);
    }

    public float ConvertTimeToSunPosition()
    {
        float timeRatio = currentTimeOfDay / 24f;

        float sunRange = 360;
        float newRatio = sunRange * timeRatio;

        float newSunPos = newRatio + sunRotationAtMidnight;

        return newSunPos;
    }
}