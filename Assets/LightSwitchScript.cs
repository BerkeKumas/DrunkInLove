using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;
    public bool isLightOn = true;

    public void SwitchPressed()
    {
        isLightOn = !isLightOn;
        lightObject.SetActive(isLightOn);
    }
}
