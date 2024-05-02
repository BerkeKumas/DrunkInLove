using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class HourTimer : MonoBehaviour
{
    public bool IsTimerActive = false;
    
    private const int GAME_OVER_SCENE_INDEX = 3;
    private const float SECONDS_TO_WAIT = 2.0f;
    private const float TOTAL_MINUTES = 60.0f;
    private const string TIME_ON_COMPLETE = "08:00";

    [SerializeField] private TextMeshProUGUI timerText;

    private float elapsedMinutes = 0;

    private void OnEnable()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {

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
