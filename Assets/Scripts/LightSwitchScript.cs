using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;
    [SerializeField] private bool isLightOn = true;

    public bool IsLightOn
    {
        get => isLightOn;
        set => UpdateLightState(value);
    }

    private void Start()
    {
        UpdateLightState(isLightOn);
    }

    private void UpdateLightState(bool value)
    {
        isLightOn = value;
        lightObject.SetActive(isLightOn);
    }
}
