using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Image staminaBar;
    [SerializeField] private Text staminaText;
    [SerializeField] private Image hungerBar;
    [SerializeField] private Text hungerText;
    [SerializeField] private Image thirstBar;
    [SerializeField] private Text thirstText;
    [SerializeField] private Text timeOfDayText;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Text difficultyText;

    public void RefreshHealthUI(float currentHealth, float maxHealth)
    {
        float scale = currentHealth / maxHealth;

        healthBar.transform.localScale = new Vector3(scale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthText.text = currentHealth.ToString("F0") + "/" + maxHealth.ToString("F0");
    }

    public void RefreshStaminaUI(float currentStamina, float maxStamina)
    {
        float scale = currentStamina / maxStamina;

        staminaBar.transform.localScale = new Vector3(scale, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
        staminaText.text = currentStamina.ToString("F0") + "/" + maxStamina.ToString("F0");
    }

    public void RefreshHungerUI(float currentHunger, float maxHunger)
    {
        float scale = currentHunger / maxHunger;

        hungerBar.transform.localScale = new Vector3(scale, hungerBar.transform.localScale.y, hungerBar.transform.localScale.z);
        hungerText.text = currentHunger.ToString("F0") + "/" + maxHunger.ToString("F0");
    }

    public void RefreshThirstUI(float currentThirst, float maxThirst)
    {
        float scale = currentThirst / maxThirst;

        thirstBar.transform.localScale = new Vector3(scale, thirstBar.transform.localScale.y, thirstBar.transform.localScale.z);
        thirstText.text = currentThirst.ToString("F0") + "/" + maxThirst.ToString("F0");
    }

    public void RefreshTimeOfDayUI(float currentHour, float currentMinute)
    {
        timeOfDayText.text = currentHour.ToString("F0") + ":" + currentMinute.ToString("F0");
    }

    public void RefreshTemperatureUI(float currentTemperature)
    {
        temperatureText.text = currentTemperature.ToString("F0") + "°";
    }

    public void RefreshDifficultyUI(float currentDifficulty)
    {
        difficultyText.text = currentDifficulty.ToString("F1") + "x";
    }
}