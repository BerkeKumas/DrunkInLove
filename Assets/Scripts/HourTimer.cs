using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class HourTimer : MonoBehaviour
{
    public bool IsTimerActive = false;

    private const int SECONDS_TO_WAIT = 2;
    private const int GAME_OVER_SCENE_INDEX = 3;
    private const float TOTAL_MINUTES = 60f;
    private const string TIME_ON_COMPLETE = "08:00";

    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        float elapsedMinutes = 0;

        while (elapsedMinutes < TOTAL_MINUTES)
        {
            if (IsTimerActive)
            {
                UpdateCountdownDisplay(elapsedMinutes);
                elapsedMinutes++;
            }
            yield return new WaitForSeconds(SECONDS_TO_WAIT);
        }

        OnTimerComplete();
    }

    private void UpdateCountdownDisplay(float elapsedMinutes)
    {
        timerText.text = string.Format("07:{0:00}", elapsedMinutes);
    }

    private void OnTimerComplete()
    {
        timerText.text = TIME_ON_COMPLETE;
        SceneManager.LoadScene(GAME_OVER_SCENE_INDEX);
    }
}
