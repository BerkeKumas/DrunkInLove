using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 2;
    [SerializeField] private float rotationSmoothing = 0.05f;

    private Vector2 rotationVelocity = Vector2.zero;
    private Vector2 frameVelocity = Vector2.zero;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        ProcessMouseMovement();
        ApplyRotation();
    }

    private void ProcessMouseMovement()
    {
        Vector2 mouseInputs = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = mouseInputs * mouseSensitivity;
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, rotationSmoothing);
        rotationVelocity += frameVelocity;
        rotationVelocity.y = Mathf.Clamp(rotationVelocity.y, -90, 90);
    }

    private void ApplyRotation()
    {
        transform.localRotation = Quaternion.AngleAxis(-rotationVelocity.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(rotationVelocity.x, Vector3.up);
    }
}
