using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField] private GameObject holdPosObj;
    private GameObject holdObject;
    private float rotationAmount = 0f;
    private bool holdObjectBool = false;
    private bool putObjectBool = false;
    private GameObject rayObject;
    private bool holdingObject = false;
    private bool putLaundry = false;
    private bool focusLaptop = false;
    private GameObject laptopObject;
    [SerializeField] private GameObject uiElement1;
    [SerializeField] private GameObject uiElement2;
    [SerializeField] private GameObject fillBarObject;
    [SerializeField] private GameObject cupObject;
    [SerializeField] private GameObject cupPosObject;
    private Vector3 cupPos;
    [SerializeField] private GameObject cameraObject;

    private void Awake()
    {
        cupPos = cupPosObject.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (focusLaptop)
            {
                if (laptopObject.GetComponent<PinScript>().enterPin == false)
                {
                    laptopObject.GetComponent<PinScript>().enterPin = true;
                }
                else
                {
                    laptopObject.GetComponent<PinScript>().enterPin = false;
                }
            }

            if (holdingObject == false)
            {
                if (rayObject != null)
                {
                    if (rayObject.tag == "fruittag" || rayObject.tag == "clothestag" || rayObject.tag == "cuptag" || rayObject.tag == "champtag")
                    {
                        GrabSound();
                        holdObject = rayObject;
                        holdObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                        uiElement1.SetActive(false);
                        holdObject.transform.parent = holdPosObj.transform;
                        holdObject.GetComponent<BoxCollider>().enabled = false;
                        holdObject.transform.localPosition = Vector3.zero;
                        holdingObject = true;
                    }
                }
            }
            else if (holdingObject)
            {
                if (holdObject.tag == "cuptag")
                {
                    GrabSound();
                    Instantiate(cupObject, cupPos, Quaternion.identity);
                    fillBarObject.GetComponent<FillBar>().DecreaseFill(50f);
                    Destroy(holdObject);
                    holdingObject = false;
                }
                else if (putLaundry)
                {
                    GrabSound();
                    Destroy(holdObject);
                    holdingObject = false;
                }
                else
                {
                    holdObject.GetComponent<Rigidbody>().isKinematic = false;
                    holdObject.GetComponent<BoxCollider>().enabled = true;
                    holdObject.transform.parent = null;
                    holdingObject = false;
                }
            }
        }

        RotateObject();

        //Pour champagne
        if (holdObject != null && holdObject.tag == "champtag")
        {
            if (rotationAmount >= 45 || rotationAmount <= -45)
            {
                holdObject.transform.GetChild(0).gameObject.SetActive(true);
                holdObject.GetComponent<ChampScript>().pourChamp = true;
            }
            else
            {
                holdObject.transform.GetChild(0).gameObject.SetActive(false);
                holdObject.GetComponent<ChampScript>().pourChamp = false;
            }
        }


        //raycast
        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);
        float rayLength = 5f;

        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            rayObject = hit.transform.gameObject;
            if (rayObject.tag == "fruittag" || rayObject.tag == "clothestag" || rayObject.tag == "cuptag" || rayObject.tag == "champtag")
            {
                uiElement1.SetActive(true);
            }
            else
            {
                uiElement1.SetActive(false);
            }

            if (rayObject.tag == "cantag" && holdObject != null)
            {
                if (holdObject.tag == "clothestag")
                {
                    uiElement2.SetActive(true);
                    putLaundry = true;
                }
            }
            else
            {
                uiElement2.SetActive(false);
                putLaundry = false;
            }

            if (rayObject.tag == "laptoptag")
            {
                focusLaptop = true;
                laptopObject = rayObject;
                laptopObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            else if (laptopObject != null)
            {
                focusLaptop = false;
                laptopObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            ClearAllDisplays();
        }

        void ClearAllDisplays()
        {
            uiElement1.SetActive(false);
            uiElement2.SetActive(false);
            if (laptopObject != null)
            {
                laptopObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            focusLaptop = false;
            putLaundry = false;
        }
    }

    private void GrabSound()
    {
        cameraObject.GetComponent<AudioSource>().Play();
    }
    private void RotateObject()
    {
        float rotateAngle = 90f;
        float maxRotation = 50f;

        if (Input.GetKey(KeyCode.Q))
        {
            if (rotationAmount < maxRotation)
            {
                float rotateStep = rotateAngle * Time.deltaTime;
                rotationAmount += rotateStep;
                rotationAmount = Mathf.Min(rotationAmount, maxRotation);
                holdObject.transform.Rotate(rotateStep, 0, 0);
                holdObject.transform.GetChild(0).rotation = Quaternion.identity;
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (rotationAmount > -maxRotation)
            {
                float rotateStep = rotateAngle * Time.deltaTime;
                rotationAmount -= rotateStep;
                rotationAmount = Mathf.Max(rotationAmount, -maxRotation);
                holdObject.transform.Rotate(-rotateStep, 0, 0);
                holdObject.transform.GetChild(0).rotation = Quaternion.identity;
            }
        }
    }
}