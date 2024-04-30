using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAfterDelay : MonoBehaviour
{
    private const float INITIAL_DELAY = 24.0f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laptopObject;
    [SerializeField] private GameObject backgroundMusic;
    [SerializeField] private GameObject hourTimerObject;
    [SerializeField] private GameObject drunkBarObject;

    private PlayerController playerController;
    private PinManager pinManager;
    private AudioSource backgroundAudio;
    private HourTimer hourTimer;
    private DrunkBar drunkBar;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        pinManager = laptopObject.GetComponent<PinManager>();
        backgroundAudio = backgroundMusic.GetComponent<AudioSource>();
        hourTimer = hourTimerObject.GetComponent<HourTimer>();
        drunkBar = drunkBarObject.GetComponent<DrunkBar>();
    }

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(INITIAL_DELAY);
        StartGame();
    }

    private void StartGame()
    {
        playerController.CanMove = true;
        pinManager.IsPinEntryActive = true;
        backgroundAudio.Play();
        hourTimer.IsTimerActive = true;
        drunkBar.StartFill = true;
        this.enabled = false;
    }

}
