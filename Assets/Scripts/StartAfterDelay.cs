using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAfterDelay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laptopObject;
    [SerializeField] private GameObject backgroundMusic;
    [SerializeField] private GameObject hourTimerObject;
    [SerializeField] private GameObject drunkBarObject;
    [SerializeField] private Material drunkEffectMaterial;

    private PlayerController playerController;
    private PinManager pinManager;
    private AudioSource backgroundAudio;
    private HourTimer hourTimer;
    private DrunkBar drunkBar;
    private PauseManager pauseManager;

    private bool isTimelineEnded = false;

    private void Awake()
    {
        drunkEffectMaterial.SetFloat("_WobbleIntensity", 0.01f);
        playerController = player.GetComponent<PlayerController>();
        pinManager = laptopObject.GetComponent<PinManager>();
        backgroundAudio = backgroundMusic.GetComponent<AudioSource>();
        hourTimer = hourTimerObject.GetComponent<HourTimer>();
        drunkBar = drunkBarObject.GetComponent<DrunkBar>();
        pauseManager = GetComponent<PauseManager>();
    }

    private void Update()
    {
        if (!isTimelineEnded) return;

        StartGame();
    }

    private void StartGame()
    {
        playerController.CanMove = true;

        pinManager.IsPinEntryActive = true;
        StartCoroutine(IncreaseAudioVolume());
        hourTimer.IsTimerActive = true;
        drunkBar.StartFill = true;
        pauseManager.canPause = true;
        this.enabled = false;
    }

    public void TimelineEnded()
    {
        isTimelineEnded = true;
    }

    private IEnumerator IncreaseAudioVolume()
    {
        while (backgroundAudio.volume < 0.4f)
        {
            backgroundAudio.volume += 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
