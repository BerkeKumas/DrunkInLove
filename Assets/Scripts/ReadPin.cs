using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadPin : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    private char[] pinCode = { '0', '4', '2', '1', '2', '0', '2', '3' };
    private char[] enteredPinColors = new char[8];
    private string enteredPin = "";
    private Tile[] tiles;
    public bool pinMode = true;
    [SerializeField] private GameObject[] Tiles;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject pinScriptObject;

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();
    }

    private int columnIndex = 0;

    void Update()
    {
        if (pinMode)
        {
            if (enteredPin.Length == 8)
            {
                CheckPinCompletion();
                for (int i = 0; i < enteredPinColors.Length; i++)
                {
                    switch (enteredPinColors[i])
                    {
                        case '1':
                            Tiles[i].GetComponent<Image>().color = Color.green;
                            break;
                        case '2':
                            Tiles[i].GetComponent<Image>().color = Color.yellow;
                            break;
                        case '3':
                            Tiles[i].GetComponent<Image>().color = Color.red;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (enteredPin.Length <= 7)
            {
                for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
                {
                    if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                    {
                        pinScriptObject.GetComponent<PinScript>().PlayKeySound();
                        tiles[columnIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                        enteredPin += (char)SUPPORTED_KEYS[i];
                        UpdateEnteredPinColors(SUPPORTED_KEYS[i]);
                        columnIndex++;
                        break;
                    }
                }
            }
            if (enteredPin.Length >= 1)
            {
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    columnIndex--;
                    enteredPinColors[columnIndex] = '0';
                    tiles[columnIndex].SetLetter((char)KeyCode.Space);
                    enteredPin = enteredPin.Remove(enteredPin.Length - 1);
                }
            }
        }
    }

    private void CheckPinCompletion()
    {
        if (enteredPinColors.All(c => c == '1'))  // Tüm renkler yeþil ise
        {
            gameManager.GetComponent<TaskManager>().musicTaskDone = true;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void UpdateEnteredPinColors(KeyCode key)
    {
        bool numberExist = pinCode.Contains((char)key);
        if (pinCode[columnIndex] == (char)key)
        {
            enteredPinColors[columnIndex] = '1';
        }
        else if (numberExist)
        {
            enteredPinColors[columnIndex] = '2';
        }
        else
        {
            enteredPinColors[columnIndex] = '3';
        }
    }
}
