using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    [SerializeField] private GameObject watermelonObject;
    [SerializeField] private GameObject cherryObject;
    [SerializeField] private GameObject redAppleObject;
    [SerializeField] private GameObject greenAppleObject;

    private bool watermelonBool = false;
    private bool cherryBool = false;
    private bool redAppleBool = false;
    private bool greenAppleBool = false;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Watermelon":
                Destroy(other.gameObject);
                watermelonObject.SetActive(true);
                watermelonBool = true;
                CheckBools();
                break;
            case "Cherry":
                Destroy(other.gameObject);
                cherryObject.SetActive(true); 
                cherryBool = true;
                CheckBools();
                break;
            case "RedApple":
                Destroy(other.gameObject);
                redAppleObject.SetActive(true); 
                redAppleBool = true;
                CheckBools();
                break;
            case "GreenApple":
                Destroy(other.gameObject);
                greenAppleObject.SetActive(true); 
                greenAppleBool = true;
                CheckBools();
                break;
            default:
                break;
        }
    }

    private void CheckBools()
    {
        if (watermelonBool && cherryBool && redAppleBool && greenAppleBool)
        {
            gameManager.GetComponent<TaskManager>().fruitTaskDone = true;
        }
    }
}
