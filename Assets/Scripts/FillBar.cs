using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FillBar : MonoBehaviour
{
    public Image fillImage;
    public float totalTime = 60f;
    [SerializeField] private TextMeshProUGUI captionTextObject;
    [SerializeField] private Image fadeImage;
    public bool startFill = false;
    private float fillSpeed = 2f;
    private float currentTime = 0f;
    Color color;

    private void Awake()
    {
        color = fadeImage.color;
    }
    void Update()
    {
        if (startFill && (currentTime < totalTime))
        {
            currentTime += Time.deltaTime * fillSpeed;
            fillImage.fillAmount = currentTime / totalTime;

            if (currentTime > 30)
            {
                color.a = (currentTime - 30) * 2 / totalTime;
                fadeImage.color = color;
                captionTextObject.text = "I need a coffee...";
            }
            else
            {
                color.a = 0;
                fadeImage.color = color;
            }


            if (currentTime >= 59)
            {
                SceneManager.LoadScene(3);
            }
        }
    }

    public void DecreaseFill(float amount)
    {
        currentTime = Mathf.Max(0, currentTime - amount);
        fillImage.fillAmount = currentTime / totalTime;
        color.a = 0;
        fadeImage.color = color;
    }
}
