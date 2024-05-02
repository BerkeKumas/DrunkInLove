using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DrunkBar : MonoBehaviour
{
    public bool StartFill = false;

    [SerializeField] private TextMeshProUGUI captionText;
    [SerializeField] private Material drunkEffectMaterial;
    [SerializeField] private Image drunkBarFillImage;
    [SerializeField] private Image transitionFadeImage;

    private float currentFillTime = 0.0f;
    private float halfFillPoint;
    private Color fadeImageColor;

    private const int MAX_FILL_DURATION = 100;
    private const int GAME_OVER_SCENE_INDEX = 3;
    private const float FILL_SPEED = 2.5f;
    private const float DRUNK_WOBBLE_FACTOR = 0.075f;
    private const float DECREASE_DURATION = 1.0f;
    private const string HALF_FILL_TEXT = "I need a coffee...";

    private void Awake()
    {
        fadeImageColor = transitionFadeImage.color;
        halfFillPoint = MAX_FILL_DURATION / 2.0f;
    }

    private void Update()
    {
        if (StartFill)
        {
            if (currentFillTime < MAX_FILL_DURATION)
            {
                currentFillTime += Time.deltaTime * FILL_SPEED;
                drunkBarFillImage.fillAmount = currentFillTime / MAX_FILL_DURATION;
                drunkEffectMaterial.SetFloat("_WobbleIntensity", currentFillTime / MAX_FILL_DURATION * DRUNK_WOBBLE_FACTOR);

                if (currentFillTime > halfFillPoint)
                {
                    UpdateColorOpacity(currentFillTime);
                    captionText.text = HALF_FILL_TEXT;
                }
                else
                {
                    ResetColorOpacity();
                }
            }
            else
            {
                SceneManager.LoadScene(GAME_OVER_SCENE_INDEX);
            }
        }
    }

    private void UpdateColorOpacity(float time)
    {
        fadeImageColor.a = (time - halfFillPoint) / halfFillPoint;
        transitionFadeImage.color = fadeImageColor;
    }

    private void ResetColorOpacity()
    {
        fadeImageColor.a = 0;
        transitionFadeImage.color = fadeImageColor;
    }

    public void DecreaseFill(float amount)
    {
        StartCoroutine(DecreaseFillOverTime(amount));
    }

    private IEnumerator DecreaseFillOverTime(float amount)
    {
        float startAmount = currentFillTime;
        float endAmount = Mathf.Max(0, currentFillTime - amount);
        float elapsedTime = 0.0f;

        while (elapsedTime < DECREASE_DURATION)
        {
            elapsedTime += Time.deltaTime;
            currentFillTime = Mathf.Lerp(startAmount, endAmount, elapsedTime / DECREASE_DURATION);
            drunkBarFillImage.fillAmount = currentFillTime / MAX_FILL_DURATION;
            yield return null;
        }

        captionText.text = string.Empty;
    }
}