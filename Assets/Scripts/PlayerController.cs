using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public bool CanMove = false;

    [SerializeField] private float moveSpeed = 10.0f;

    private PlayerInputActions playerInputActions;
    private AudioSource walkingSound;


    private void Awake()
    {
        walkingSound = GetComponent<AudioSource>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Update()
    {
        if (!CanMove) return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>().normalized;

        Vector3 movement = moveSpeed * Time.deltaTime * (transform.forward * inputVector.y + transform.right * inputVector.x);
        transform.position += movement;

        bool isWalking = inputVector != Vector2.zero;
        UpdateWalkingSound(isWalking);
    }

    private void UpdateWalkingSound(bool isWalking)
    {
        if (isWalking)
        {
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
    }
}
