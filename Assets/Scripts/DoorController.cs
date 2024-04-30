using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isDoorOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Awake()
    {
        closedRotation = transform.rotation;
        openRotation = transform.rotation * Quaternion.Euler(0, -90f, 0);
    }

    public void ToggleDoor()
    {
        StartCoroutine(RotateDoor(isDoorOpen));
        isDoorOpen = !isDoorOpen;
    }

    private IEnumerator RotateDoor(bool isOpen)
    {
        float duration = 1.0f;
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion endRotation = isOpen ? closedRotation : openRotation;

        float time = 0.0f;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
    public bool IsDoorOpen()
    {
        return isDoorOpen;
    }
}
