using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CaptionTextTyper : MonoBehaviour
{
    private TextMeshProUGUI textDisplay;
    private float delay = 0.075f;
    private string fullText;
    private string currentFullText;
    private string currentText = "";
    private bool canType = true;
    private bool isStable = false;
    private bool currentIsStable = false;


    private void Awake()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    public void StartType(string _fullText, bool isTextStable)
    {
        fullText = _fullText;
        isStable = isTextStable;
        if (canType && currentFullText != fullText)
        {
            canType = false;
            currentFullText = fullText;
            currentIsStable = isStable;
            textDisplay.text = currentText;
            StartCoroutine(ShowText());
        }
    }

    public void ResetTextIfEqual(string text)
    {
        if (text == fullText)
        {
            StartType("", true);
        }
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i <= currentFullText.Length; i++)
        {
            currentText = currentFullText.Substring(0, i);
            textDisplay.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(delay * 15f);

        if (currentFullText != fullText)
        {
            currentText = "";
            textDisplay.text = currentText;
            currentFullText = fullText;
            StartCoroutine(ShowText());
        }
        else
        {
            if (!currentIsStable)
            {
                yield return new WaitForSeconds(delay * 30f);
                currentText = "";
                textDisplay.text = currentText;
            }
            canType = true;
        }
    }
}
