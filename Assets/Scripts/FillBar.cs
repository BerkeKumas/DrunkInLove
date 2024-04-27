using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public Material drunkMaterial;

    [Range(0.0f, 1.0f)]
    public float wobbleIntensity = 1f;


    private void Awake()
    {
        color = fadeImage.color;
    }
    private void Update()
    {
        if (startFill && (currentTime < totalTime))
        {
            currentTime += Time.deltaTime * fillSpeed;
            fillImage.fillAmount = currentTime / totalTime;
            drunkMaterial.SetFloat("_WobbleIntensity", currentTime / totalTime * 0.075f);

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
        StartCoroutine(DecreaseFillOverTime(amount, 1.0f));
    }

    private IEnumerator DecreaseFillOverTime(float amount, float duration)
    {
        float startAmount = currentTime;
        float endAmount = Mathf.Max(0, currentTime - amount);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentTime = Mathf.Lerp(startAmount, endAmount, elapsedTime / duration);
            fillImage.fillAmount = currentTime / totalTime;
            yield return null;
        }

        captionTextObject.text = "";
        currentTime = endAmount;
        fillImage.fillAmount = currentTime / totalTime;
        color.a = 0;
        fadeImage.color = color;
    }
}
