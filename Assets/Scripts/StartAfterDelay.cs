using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAfterDelay : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laptop;
    [SerializeField] private GameObject backgroundAudioObject;

    private void Start()
    {
        StartCoroutine(Delay());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(24);
        StartGame();
    }

    private void StartGame()
    {
        gameObject.GetComponent<CountdownTimer>().startTimer = true;
        gameObject.GetComponent<FillBar>().startFill = true;
        player.GetComponent<Player>().playerMovement = true;
        laptop.GetComponent<PinScript>().startPin = true;
        backgroundAudioObject.GetComponent<AudioSource>().Play();
        this.enabled = false;
    }

}
