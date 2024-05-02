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
    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        defaultPosY = transform.localPosition.y;
        defaultFOV = camera.fieldOfView;
    }

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cSharpTransform = transform.localPosition;

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
            cSharpTransform.y = defaultPosY + translateChange;
        }
        else
        {
            cSharpTransform.y = defaultPosY;
        }

        transform.localPosition = cSharpTransform;


        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? defaultFOV + runningFOVIncrease : defaultFOV;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFOV, fovSmoothTime * Time.deltaTime);
    }
}
