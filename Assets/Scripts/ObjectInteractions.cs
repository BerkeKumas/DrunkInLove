using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    private readonly Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f);

    private const int RAY_LENGTH = 5;
    private const string LAST_LAUNDRY_TEXT = "I think something fell on the ground.";
    private const string HOLDING_KEY_TEXT = "A key I wonder where this opens.";

    [SerializeField] private GameObject taskManager;
    [SerializeField] private GameObject holdObjectParent;
    [SerializeField] private GameObject zoomObjectParent;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject fillBarObject;
    [SerializeField] private GameObject cupObject;
    [SerializeField] private GameObject cupPosObject;
    [SerializeField] private GameObject itemSoundObject;
    [SerializeField] private GameObject keyObject;
    [SerializeField] private AudioClip[] itemSounds;
    [SerializeField] private TextMeshProUGUI captionTextObject;

    private bool holdingObject = false;
    private bool dropLaundry = false;
    private bool openLaptop = false;
    private bool isDoorLocked = true;
    private bool doorControl = false;
    private bool unlockDoor = false;
    private bool controlLight = false;
    private GameObject holdObject;
    private GameObject rayObject;
    private GameObject laptopObject;
    private GameObject zoomObject;
    private Vector3 cupPos;

    private void Awake()
    {
        cupPos = cupPosObject.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (openLaptop)
            {
                laptopObject.GetComponent<PinManager>().ShouldEnterPin = !laptopObject.GetComponent<PinManager>().ShouldEnterPin;
            }
            else if (controlLight)
            {
                rayObject.GetComponent<LightSwitchScript>().IsLightOn = !rayObject.GetComponent<LightSwitchScript>().IsLightOn;
            }
            else if (dropLaundry)
            {
                ItemSounds(0);
                DestroyObject(holdObject);
            }
            else if (unlockDoor)
            {
                unlockDoor = false;
                isDoorLocked = false;
                DestroyObject(holdObject);
            }
            else if (!isDoorLocked && doorControl)
            {
                rayObject.GetComponent<DoorController>().ToggleDoor();
            }
            else if (holdingObject)
            {
                HoldingObjectActions();
            }
            else
            {
                RaycastObjectActions();
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, RAY_LENGTH))
        {
            rayObject = hit.transform.gameObject;
            HandleRaycastHit(rayObject);
        }
        else
        {
            ClearAllDisplays();
        }
    }

    private void HandleRaycastHit(GameObject rayObject)
    {
        switch (rayObject.tag)
        {
            case "lastlaundrytag":
            case "sewergratetag":
            case "flashlighttag":
            case "clothestag":
            case "fruittag":
            case "winetag":
            case "keytag":
            case "cuptag":
                interactionText.text = "Press \"F\" to Hold Object.";
                break;
            case "laundryboxtag":
                HandleLaundryBoxInteraction();
                break;
            case "doortag":
                HandleDoorInteraction();
                break;
            case "zoomtag":
                interactionText.text = "Press \"F\" to Zoom.";
                break;
            case "lightswitchtag":
                controlLight = true;
                interactionText.text = rayObject.GetComponent<LightSwitchScript>().IsLightOn ? "Press \"F\" to Switch Off Lights." : "Press \"F\" to Switch On Lights.";
                break;
            case "laptoptag":
                openLaptop = true;
                laptopObject = rayObject;
                laptopObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                break;
            default:
                ClearAllDisplays();
                break;
        }
    }

    private void HandleLaundryBoxInteraction()
    {
        if (holdObject is { tag: "clothestag" or "lastlaundrytag" })
        {
            interactionText.text = "Press \"F\" to Put Clothes into Laundry Box.";
            dropLaundry = true;
        }
    }

    private void HandleDoorInteraction()
    {
        doorControl = true;
        if (isDoorLocked)
        {
            if (holdObject is { tag: "keytag" })
            {
                interactionText.text = "Press \"F\" to Unlock.";
                unlockDoor = true;
            }
            else
            {
                interactionText.text = "Looks like it's locked.";
            }
        }
        else
        {
            interactionText.text = rayObject.GetComponent<DoorController>().IsDoorOpen() ? "Press \"F\" to Close." : "Press \"F\" to Open.";
        }
    }

    private void ClearAllDisplays()
    {
        interactionText.text = string.Empty;
        if (laptopObject != null)
        {
            laptopObject.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        controlLight = false;
        doorControl = false;
        dropLaundry = false;
        openLaptop = false;
        unlockDoor = false;
    }

    private void HoldingObjectActions()
    {
        if (dropLaundry)
        {
            ItemSounds(0);
            DestroyObject(holdObject);
        }
        else if (holdObject.tag == "cuptag")
        {
            ItemSounds(1);
            Instantiate(cupObject, cupPos, Quaternion.identity);
            fillBarObject.GetComponent<DrunkBar>().DecreaseFill(75.0f);
            DestroyObject(holdObject);
        }
        else if (holdObject.tag == "zoomtag")
        {
            DestroyObject(zoomObject);
        }
        else
        {
            if (holdObject.tag == "flashlighttag")
            {
                holdObject.GetComponent<Light>().enabled = false;
            }
            holdObject.GetComponent<Rigidbody>().isKinematic = false;
            holdObject.GetComponent<BoxCollider>().enabled = true;
            holdObject.transform.parent = null;
            holdingObject = false;
            holdObject = null;
        }
    }

    private void RaycastObjectActions()
    {
        if (rayObject is { tag: "sewergratetag" or "fruittag" or "clothestag" or "cuptag" })
        {
            HoldObject();
        }
        else if (rayObject is { tag: "winetag"})
        {
            HoldObject();
            rayObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (rayObject is { tag: "flashlighttag" })
        {
            HoldObject();
            rayObject.GetComponent<Light>().enabled = true;
            rayObject.transform.localEulerAngles = new Vector3(-15, 0, 0);
            rayObject.transform.localPosition = new Vector3(0.7f, -0.75f, -0.75f);
        }
        else if (rayObject is { tag: "lastlaundrytag" })
        {
            HoldObject();
            DropKey();
            taskManager.GetComponent<TaskManager>().lastLaundryActive = true;
            captionTextObject.text = LAST_LAUNDRY_TEXT;
        }
        else if (rayObject is { tag: "keytag" })
        {
            HoldObject();
            captionTextObject.text = HOLDING_KEY_TEXT;

        }
        else if (rayObject is { tag: "zoomtag" })
        {
            zoomObject = Instantiate(rayObject, zoomObjectParent.transform.position, Quaternion.identity, zoomObjectParent.transform);
            zoomObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            holdObject = zoomObject.gameObject;
            holdingObject = true;
        }
    }

    private void HoldObject()
    {
        ItemSounds(0);
        holdObject = rayObject;
        holdObject.transform.GetComponent<Rigidbody>().isKinematic = true;
        holdObject.transform.parent = holdObjectParent.transform;
        holdObject.GetComponent<BoxCollider>().enabled = false;
        holdObject.transform.localPosition = Vector3.zero;
        holdingObject = true;
    }

    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
        holdObject = null;
        holdingObject = false;
    }

    private void ItemSounds(int index)
    {
        itemSoundObject.GetComponent<AudioSource>().clip = itemSounds[index];
        itemSoundObject.GetComponent<AudioSource>().Play();
    }

    private void DropKey()
    {
        Instantiate(keyObject, holdObjectParent.transform.position, Quaternion.identity);
    }
}