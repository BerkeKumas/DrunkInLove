using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.TextCore.Text;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    private PlayerInputActions playerInputActions;
    public bool playerMovement = false;
    AudioSource walkingAudio;
    private bool soundPlaying = false;


    private void Awake()
    {
        walkingAudio = GetComponent<AudioSource>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }

    private void Update()
    {
        if (playerMovement)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        transform.position += moveSpeed * transform.forward * inputVector.y * Time.deltaTime;
        transform.position += moveSpeed * transform.right * inputVector.x * Time.deltaTime;

        bool isWalking = inputVector != Vector2.zero;
        if (isWalking)
        {
            if (!soundPlaying)
            {
                walkingAudio.Play();
                soundPlaying = true;
            }
        }
        else
        {
            walkingAudio.Stop();
            soundPlaying = false;
        }
    }
}
