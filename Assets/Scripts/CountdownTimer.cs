using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private float remainingMinutes = 60f;

    void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        float passedMinutes = 0;

        while (passedMinutes < remainingMinutes)
        {
            int currentHour = 7 + Mathf.FloorToInt(passedMinutes / 60);
            int currentMinute = Mathf.FloorToInt(passedMinutes % 60);

            countdownText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);

            yield return new WaitForSeconds(2f);
            passedMinutes += 1;
        }

        SceneManager.LoadScene(3);
        countdownText.text = "08:00";
    }
}
