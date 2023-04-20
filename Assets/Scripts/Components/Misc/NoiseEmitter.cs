using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    // Tracks how far away the noise can be heard from
    [SerializeField] private float walkVolumeDistance;
    [SerializeField] private float sprintVolumeDistance;

    // Used to store either 0 or the values from the other variables
    private float currentNoiseDistance = 0f;

    public void IsWalking()
    {
        currentNoiseDistance = walkVolumeDistance;
    }

    public void IsSprinting()
    {
        currentNoiseDistance = sprintVolumeDistance;
    }

    // Sets noise back to 0
    public void IsSilent()
    {
        currentNoiseDistance = 0f;
    }

    public float GetCurrentNoiseDistance()
    {
        return currentNoiseDistance;
    }
}