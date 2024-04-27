using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PickObjects : MonoBehaviour
{
    [SerializeField] private GameObject taskManager;
    [SerializeField] private GameObject holdObjectParent;
    [SerializeField] private GameObject zoomObjectParent;
    [SerializeField] private GameObject uiElement1;
    [SerializeField] private GameObject uiElement2;
    [SerializeField] private GameObject uiElement3;
    [SerializeField] private GameObject uiElement4;
    [SerializeField] private GameObject uiElement5;
    [SerializeField] private GameObject uiElement6;
    [SerializeField] private GameObject uiElement7;
    [SerializeField] private GameObject uiElement8;
    [SerializeField] private GameObject uiElement9;
    [SerializeField] private GameObject fillBarObject;
    [SerializeField] private GameObject cupObject;
    [SerializeField] private GameObject cupPosObject;
    [SerializeField] private GameObject itemSoundObject;
    [SerializeField] private GameObject keyObject;
    [SerializeField] private AudioClip[] itemSounds;
    [SerializeField] private TextMeshProUGUI captionTextObject;
    private GameObject holdObject;
    private GameObject rayObject;
    private GameObject laptopObject;
    private GameObject zoomObject;
    private GameObject doorObject;
    private float rotationAmount = 0f;
    private bool holdingObject = false;
    private bool putLaundry = false;
    private bool focusLaptop = false;
    private bool holdingKeyToDoor = false;
    private bool isDoorOpen = false;
    private bool isDoorLocked = true;
    private Vector3 cupPos;
    private Quaternion closedRotation;
    private Quaternion openRotation;

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
                laptopObject.GetComponent<PinScript>().enterPin = !laptopObject.GetComponent<PinScript>().enterPin;
            }

            if (rayObject.tag == "doortag" && !isDoorLocked)
            {
                ToggleDoor();
            }
            else if (rayObject.tag == "lightswitchtag")
            {
                rayObject.GetComponent<LightSwitchScript>().SwitchPressed();
            }
            else if (holdingObject == false)
            {
                if (rayObject != null)
                {
                    if (rayObject.tag == "lastlaundrytag" || rayObject.tag == "keytag" || rayObject.tag == "fruittag" || rayObject.tag == "clothestag" || rayObject.tag == "cuptag" || rayObject.tag == "champtag" || rayObject.tag == "flashlighttag")
                    {
                        ItemSounds(0);
                        holdObject = rayObject;
                        holdObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                        uiElement1.SetActive(false);
                        holdObject.transform.parent = holdObjectParent.transform;
                        holdObject.GetComponent<BoxCollider>().enabled = false;
                        holdObject.transform.localPosition = Vector3.zero;
                        holdingObject = true;
                        if (rayObject.tag == "flashlighttag")
                        {
                            rayObject.GetComponent<Light>().enabled = true;
                            rayObject.transform.localEulerAngles = new Vector3(-15, 0, 0);
                            rayObject.transform.localPosition = new Vector3(0.7f, -0.75f, -0.75f);
                        }
                        else if (rayObject.tag == "lastlaundrytag")
                        {
                            DropKey();
                            taskManager.GetComponent<TaskManager>().lastLaundryActive = true;
                            captionTextObject.text = "I think something fell on the ground.";
                        }
                        else if (rayObject.tag == "keytag")
                        {
                            captionTextObject.text = "A key I wonder where this opens.";
                        }
                    }
                    else if (rayObject.tag == "zoomtag")
                    {
                        zoomObject = Instantiate(rayObject, zoomObjectParent.transform.position, Quaternion.identity, zoomObjectParent.transform);
                        zoomObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                        holdObject = zoomObject.gameObject;
                        holdingObject = true;
                    }
                }
            }
            else if (holdingObject)
            {
                if (holdObject.tag == "cuptag")
                {
                    ItemSounds(1);
                    Instantiate(cupObject, cupPos, Quaternion.identity);
                    fillBarObject.GetComponent<FillBar>().DecreaseFill(50f);
                    Destroy(holdObject);
                    holdingObject = false;
                }
                else if (putLaundry)
                {
                    ItemSounds(0);
                    Destroy(holdObject);
                    holdingObject = false;
                }
                else if (holdObject.tag == "zoomtag")
                {
                    Destroy(zoomObject.gameObject);
                    holdingObject = false;
                }
                else
                {
                    holdObject.GetComponent<Rigidbody>().isKinematic = false;
                    holdObject.GetComponent<BoxCollider>().enabled = true;
                    holdObject.transform.parent = null;
                    holdingObject = false;
                    holdObject = null;
                    if (rayObject.tag == "flashlighttag")
                    {
                        rayObject.GetComponent<Light>().enabled = false;
                    }
                    else if (holdingKeyToDoor)
                    {
                        holdingKeyToDoor = false;
                        uiElement4.SetActive(true);
                        uiElement6.SetActive(false);
                        isDoorLocked = false;
                    }
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
            if (rayObject.tag == "keytag" || rayObject.tag == "fruittag" || rayObject.tag == "clothestag" || rayObject.tag == "cuptag" || rayObject.tag == "champtag" || rayObject.tag == "flashlighttag" || rayObject.tag == "lastlaundrytag")
            {
                uiElement1.SetActive(true);
            }
            else if (rayObject.tag == "laundryboxtag" && holdObject != null)
            {
                if (holdObject.tag == "clothestag" || holdObject.tag == "lastlaundrytag")
                {
                    uiElement2.SetActive(true);
                    putLaundry = true;
                }
            }
            else if (rayObject.tag == "doortag")
            {
                uiElement1.SetActive(false);
                if (holdObject != null)
                {
                    if (isDoorLocked)
                    {
                        if (holdObject.tag == "keytag")
                        {
                            uiElement6.SetActive(true);
                            doorObject = rayObject.gameObject;
                            closedRotation = doorObject.transform.rotation;
                            openRotation = doorObject.transform.rotation * Quaternion.Euler(0, -90f, 0);
                            holdingKeyToDoor = true;
                        }
                        else
                        {
                            uiElement5.SetActive(true);
                        }
                    }
                    else
                    {
                        uiElement5.SetActive(false);
                        uiElement6.SetActive(false);
                        if (!isDoorOpen)
                        {
                            uiElement7.SetActive(false);
                            uiElement4.SetActive(true);
                        }
                        else
                        {
                            uiElement4.SetActive(false);
                            uiElement7.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (isDoorLocked)
                    {
                        uiElement5.SetActive(true);
                    }
                    else
                    {
                        uiElement5.SetActive(false);
                        if (!isDoorOpen)
                        {
                            uiElement7.SetActive(false);
                            uiElement4.SetActive(true);
                        }
                        else
                        {
                            uiElement4.SetActive(false);
                            uiElement7.SetActive(true);
                        }
                    }
                }
            }
            else if (rayObject.tag == "zoomtag")
            {
                uiElement3.SetActive(true);
            }
            else if (rayObject.tag == "lightswitchtag")
            {
                if (rayObject.GetComponent<LightSwitchScript>().isLightOn)
                {
                    uiElement9.SetActive(false);
                    uiElement8.SetActive(true);
                }
                else
                {
                    uiElement8.SetActive(false);
                    uiElement9.SetActive(true);
                }
            }
            else
            {
                uiElement1.SetActive(false);
                uiElement2.SetActive(false);
                uiElement3.SetActive(false);
                uiElement4.SetActive(false);
                uiElement5.SetActive(false);
                uiElement6.SetActive(false);
                uiElement7.SetActive(false);
                uiElement8.SetActive(false);
                uiElement9.SetActive(false);
                putLaundry = false;
                holdingKeyToDoor = false;
            }

            if (rayObject.tag == "laptoptag")
            {
                focusLaptop = true;
                laptopObject = rayObject;
                laptopObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            }
            else if (laptopObject != null)
            {
                focusLaptop = false;
                laptopObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
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
            uiElement3.SetActive(false);
            uiElement4.SetActive(false);
            uiElement5.SetActive(false);
            uiElement6.SetActive(false);
            uiElement7.SetActive(false);
            uiElement8.SetActive(false);
            uiElement9.SetActive(false);
            if (laptopObject != null)
            {
                laptopObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            }
            focusLaptop = false;
            putLaundry = false;
            holdingKeyToDoor = false;
        }
    }

    private void ItemSounds(int index)
    {
        itemSoundObject.GetComponent<AudioSource>().clip = itemSounds[index];
        itemSoundObject.GetComponent<AudioSource>().Play();
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

    private void DropKey()
    {
        Instantiate(keyObject, holdObjectParent.transform.position, Quaternion.identity);
    }

    public void ToggleDoor()
    {
        StartCoroutine(RotateDoor(isDoorOpen));
        isDoorOpen = !isDoorOpen;
    }

    IEnumerator RotateDoor(bool isOpen)
    {
        float duration = 1.0f;
        Quaternion startRotation = doorObject.transform.rotation;
        Quaternion endRotation = isOpen ? closedRotation : openRotation;

        float time = 0.0f;
        while (time < duration)
        {
            doorObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        doorObject.transform.rotation = endRotation;
    }
}