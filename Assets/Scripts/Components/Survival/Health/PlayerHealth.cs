using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerHealth : Health
{
    #region OverrideFunctions
    public override void DecCurrentValue(float damage)
    {
        base.DecCurrentValue(damage);

        if (currentValue <= 0)
        {
            MenuManager.instance.ShowDeathScreen();
            Debug.Log("Player died");
            GameManager.instance.SetCurrentPlayerController(null);

            Die();
        }
    }

    public override void DecCurrentValueOverTime()
    {
        base.DecCurrentValueOverTime();

        if (currentValue <= 0)
        {
            MenuManager.instance.ShowDeathScreen();
            Debug.Log("Player died");
            GameManager.instance.SetCurrentPlayerController(null);

            Die();
        }
    }

    #endregion OverrideFunctions
}