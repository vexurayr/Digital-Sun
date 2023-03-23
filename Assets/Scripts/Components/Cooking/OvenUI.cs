using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenUI : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject fuelInputSlot;
    [SerializeField] private GameObject convertInputSlot;
    [SerializeField] private GameObject outputSlot;

    [SerializeField] private GameObject fuelInputCounter;
    [SerializeField] private GameObject convertInputCounter;
    [SerializeField] private GameObject outputCounter;

    [SerializeField] private GameObject fuelInputItem;
    [SerializeField] private GameObject convertInputItem;
    [SerializeField] private GameObject outputItem;

    #endregion Variables

    #region GetSet
    public GameObject GetFuelInputSlot()
    {
        return fuelInputSlot;
    }

    public GameObject GetConvertInputSlot()
    {
        return convertInputSlot;
    }

    public GameObject GetOutputSlot()
    {
        return outputSlot;
    }

    public GameObject GetFuelInputCounter()
    {
        return fuelInputCounter;
    }

    public GameObject GetConvertInputCounter()
    {
        return convertInputCounter;
    }

    public GameObject GetOutputCounter()
    {
        return outputCounter;
    }

    public GameObject GetFuelInputItem()
    {
        return fuelInputItem;
    }

    public GameObject GetConvertInputItem()
    {
        return convertInputItem;
    }

    public GameObject GetOutputItem()
    {
        return outputItem;
    }

    #endregion GetSet
}