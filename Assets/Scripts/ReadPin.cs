using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ReadPin : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    private char[] pinCode = new char[8];
    private char[] enteredPin = new char[8];
    private int columnIndex = 0;
    public bool pinMode = false;

    [SerializeField] private GameObject[] Tiles;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject pinScriptObject;
    [SerializeField] private GameObject backgroundAudioObject;
    [SerializeField] private TextMeshProUGUI dateText;

    private void Awake()
    {
        for (int i = 0; i < enteredPin.Length; i++)
        {
            enteredPin[i] = '\0';
        }

        System.Random random = new System.Random();

        int month = random.Next(1, 13);
        char[] monthChars = month.ToString("D2").ToCharArray();

        int day = random.Next(1, 29);
        char[] dayChars = day.ToString("D2").ToCharArray();

        int year = random.Next(2000, 2024);
        char[] yearChars = year.ToString().ToCharArray();

        pinCode[0] = monthChars[0];
        pinCode[1] = monthChars[1];
        pinCode[2] = dayChars[0];
        pinCode[3] = dayChars[1];
        pinCode[4] = yearChars[0];
        pinCode[5] = yearChars[1];
        pinCode[6] = yearChars[2];
        pinCode[7] = yearChars[3];

        dateText.text = $"{pinCode[0]}{pinCode[1]}.{pinCode[2]}{pinCode[3]}.{pinCode[4]}{pinCode[5]}{pinCode[6]}{pinCode[7]}";
    }

    void Update()
    {
        if (!pinMode) return;

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && columnIndex > 0)
        {
            columnIndex--;
            enteredPin[columnIndex] = '\0';
            Tiles[columnIndex].GetComponent<Image>().color = Color.white;
            Tiles[columnIndex].GetComponent<Tile>().SetLetter(' ');
            pinScriptObject.GetComponent<PinScript>().PlayKeySound();
        }
        foreach (KeyCode key in SUPPORTED_KEYS)
        {
            if (Input.GetKeyDown(key) && columnIndex < 8)
            {
                char enteredChar = (char)('0' + key - KeyCode.Alpha0);
                enteredPin[columnIndex] = enteredChar;
                Tiles[columnIndex].GetComponent<Tile>().SetLetter(enteredChar);
                UpdateColor(columnIndex, enteredChar);
                columnIndex++;

                if (columnIndex == 8)
                {
                    CheckPinCompletion();
                }
                break;
            }
        }
    }

    private void UpdateColor(int index, char enteredChar)
    {
        if (pinCode[index] == enteredChar)
        {
            Tiles[index].GetComponent<Image>().color = Color.green;
        }
        else
        {
            Tiles[index].GetComponent<Image>().color = Color.red;
        }
    }

    private void CheckPinCompletion()
    {
        bool pinCorrect = true;
        for (int i = 0; i < pinCode.Length; i++)
        {
            if (enteredPin[i] != pinCode[i])
            {
                pinCorrect = false;
                break;
            }
        }

        if (pinCorrect)
        {
            gameManager.GetComponent<TaskManager>().musicTaskDone = true;
            backgroundAudioObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<AudioSource>().Play();
            pinMode = false;
        }
    }
}
