using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    private const float WALK_SPEED = 10.0f;
    private const float RUN_SPEED = 13.0f;
    private const float CROUCH_SPEED = 5.0f;
    private const float SPEED_LERP_FACTOR = 2.0f;
    private const float STAND_SCALE_Y = 1.0f;
    private const float CROUCH_SCALE_Y = 0.5f;

    public bool CanMove = true;

    [SerializeField] private float moveSpeed = WALK_SPEED;

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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else
        {
            Stand();
        }
        MovePlayer();
    }
    private void Crouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, CROUCH_SCALE_Y, transform.localScale.z);
        moveSpeed = CROUCH_SPEED;
    }

    private void Stand()
    {
        transform.localScale = new Vector3(transform.localScale.x, STAND_SCALE_Y, transform.localScale.z);
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? RUN_SPEED : WALK_SPEED;
        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, SPEED_LERP_FACTOR * Time.deltaTime);
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
