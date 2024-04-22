using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PinScript : MonoBehaviour
{

    public bool enterPin = false;
    private bool pinModeBool = false;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject pinCamera;
    [SerializeField] private GameObject player;
    private Player playerScript;
    [SerializeField] private GameObject pinBoard;
    [SerializeField] private AudioClip[] keySounds;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {

        if (enterPin)
        {
            playerScript.playerMovement = false;
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            pinCamera.SetActive(true);
            mainCamera.SetActive(false);
            pinBoard.GetComponent<ReadPin>().pinMode = true;
        }
        else
        {
            playerScript.playerMovement = true;
            pinModeBool = false;
            mainCamera.SetActive(true);
            pinCamera.SetActive(false);
            pinBoard.GetComponent<ReadPin>().pinMode = false;
        }
    }

    public void PlayKeySound()
    {
        int rand = Random.Range(0, keySounds.Length);
        gameObject.GetComponent<AudioSource>().clip = keySounds[rand];
        gameObject.GetComponent<AudioSource>().Play();

    }

}
