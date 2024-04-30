using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PinManager : MonoBehaviour
{
    public bool IsPinEntryActive = false;
    public bool ShouldEnterPin = false;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject pinCamera;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject pinEntryPanel;
    [SerializeField] private AudioClip[] keypadSounds;

    private PlayerController playerController;
    private AudioSource audioSource;

    private void Awake()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsPinEntryActive)
        {
            ManagePinEntry();
        }
    }

    private void ManagePinEntry()
    {
        if (ShouldEnterPin)
        {
            ActivatePinMode();
        }
        else
        {
            DeactivatePinMode();
        }
    }

    private void ActivatePinMode()
    {
        playerController.CanMove = false;
        pinCamera.SetActive(true);
        mainCamera.SetActive(false);
        pinEntryPanel.GetComponent<ReadPin>().PinMode = true;
    }

    private void DeactivatePinMode()
    {
        playerController.CanMove = true;
        pinCamera.SetActive(false);
        mainCamera.SetActive(true);
        pinEntryPanel.GetComponent<ReadPin>().PinMode = false;
    }

    public void PlayKeySound()
    {
        int randomIndex = UnityEngine.Random.Range(0, keypadSounds.Length);
        audioSource.clip = keypadSounds[randomIndex];
        audioSource.Play();
    }
}