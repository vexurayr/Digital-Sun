using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Powerup;

public class Armor : InventoryItem
{
    #region Variables
    [Tooltip("This number will be a percentage that reduces incoming damage.")]
        [Range(0, 1)][SerializeField] private float defenseProtection;
    [Tooltip("Flat increase to the range of cold temperatures that are safe for the player.")]
        [SerializeField] private float coldTemperatureProtection;
    [Tooltip("Flat increase to the range of hot temperatures that are safe for the player.")]
        [SerializeField] private float hotTemperatureProtection;

    #endregion Variables

    // Slightly different for armor, only using PowerupManager for player reference
    // Primary Action gives player the armor's buffs
    #region PrimaryAction
    public override bool PrimaryAction(GameObject player)
    {
        Defense defense = player.GetComponent<Defense>();

        defense.IncCurrentValue(defenseProtection);
        defense.IncCurrentHotProtection(hotTemperatureProtection);
        defense.IncCurrentColdProtection(coldTemperatureProtection);

        return true;
    }

    #endregion PrimaryAction

    // Second Action removes the armor's buffs
    #region SecondAction
    public override bool SecondaryAction(GameObject player)
    {
        Defense defense = player.GetComponent<Defense>();

        defense.DecCurrentValue(defenseProtection);
        defense.DecCurrentHotProtection(hotTemperatureProtection);
        defense.DecCurrentColdProtection(coldTemperatureProtection);

        return false;
    }

    #endregion SecondAction
}