using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classify : MonoBehaviour
{
    public enum Classification
    {
        None,
        IsFuelSource,
        IsConvertable,
        IsOvenFuelInput,
        IsOvenConvertInput,
        IsOvenOutput
    }

    [SerializeField] private Classification classification;

    public Classification GetClassification()
    { 
        return classification;
    }
}