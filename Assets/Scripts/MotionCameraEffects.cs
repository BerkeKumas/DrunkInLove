using UnityEngine;

public class MotionCameraEffects : MonoBehaviour
{
    private float walkingBobbingSpeed = 14f;
    private float bobbingAmount = 0.1f;
    private float runningFOVIncrease = 15f;
    private float fovSmoothTime = 2f;
    private float defaultFOV;
    private float defaultPosY = 0;
    private float timer = 0;
    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        defaultPosY = transform.localPosition.y;
        defaultFOV = _camera.fieldOfView;
    }

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraTransform = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + walkingBobbingSpeed * Time.deltaTime;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            cameraTransform.y = defaultPosY + translateChange;
        }
        else
        {
            cameraTransform.y = defaultPosY;
        }

        transform.localPosition = cameraTransform;

        UpdateFOV();
    }

    private void UpdateFOV()
    {
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? defaultFOV + runningFOVIncrease : defaultFOV;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, fovSmoothTime * Time.deltaTime);
    }
}
