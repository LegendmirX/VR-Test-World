using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour 
{
    public bool DebugMode = false;

    public bool ShowController = false;

    [SerializeField] private List<GameObject> controllerPrefabs = null;
    [SerializeField] private GameObject handPrefab = null;

    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;

    private GameObject controllerGo = null;
    private GameObject handGameObject = null;

    private Animator handAnimator;

    private InputDevice controller;
    private bool controllerFound = false;

    #region ButtonData
    private bool primary2DAxisPressed = false;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        List<InputDevice> tempList = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, tempList);

        if (tempList.Count > 0)
        {
            controller = tempList[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == this.controller.name);
            if (prefab != null)
            {
                controllerGo = Instantiate(prefab, this.transform);
            }
            else
            {
                Debug.LogError("Controller '" + controller.name + "' not found");
                controllerGo = Instantiate(controllerPrefabs[0], this.transform);
            }

            handGameObject = Instantiate(handPrefab, this.transform);
            handAnimator = handGameObject.GetComponent<Animator>();

            if (ShowController == true)
            {
                controllerGo.SetActive(true);
                handGameObject.SetActive(false);
            }
            else
            {
                controllerGo.SetActive(false);
                handGameObject.SetActive(true);
            }

            controllerFound = true;
        } //Set and spawn Controller
    }

    // Update is called once per frame
    void Update()
    {
        if(controllerFound == false)
        {
            TryInitialize();
            return;
        }

        if(ShowController == true)
        {
            if(controllerGo.activeSelf == false)
            {
                controllerGo.SetActive(true);
                handGameObject.SetActive(false);
            }
            UpdateAnimation();
        }
        else
        {
            if(handGameObject.activeSelf == false)
            {
                controllerGo.SetActive(false);
                handGameObject.SetActive(true);
            }
            UpdateHandAnimation();
        }
    }

    private void UpdateHandAnimation()
    {
        float triggerValue;
        float gripValue;

        if (controller.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            triggerValue = -1;
            handAnimator.SetFloat("Trigger", 0);
        }

        if (controller.TryGetFeatureValue(CommonUsages.grip, out gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            gripValue = -1;
            handAnimator.SetFloat("Grip", 0);
        }

        if (DebugMode == true)
        {
            Debug.Log("Trigger: " + triggerValue);
            Debug.Log("Grip: " + gripValue);
        }

        controller.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primary2DAxisTouch);
        controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary2DAxisPress);
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisTouch == true)
        {
            if (DebugMode == true)
            {
                Debug.Log("primary2DAxis: " + primary2DAxisValue);
            }
        }

        if (primary2DAxisPress == true && primary2DAxisPressed == false)
        {
            primary2DAxisPressed = true;
            if (DebugMode == true)
            {
                Debug.Log("primary2DAxisPressed: " + primary2DAxisValue);
            }
        }
        else if (primary2DAxisPress == false && primary2DAxisPressed == true)
        {
            primary2DAxisPressed = false;
        }
    }

    private void UpdateAnimation()
    {
        float triggerValue;
        float gripValue;

        if(controller.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {

        }
        else
        {
            triggerValue = -1;
        }

        if (controller.TryGetFeatureValue(CommonUsages.grip, out gripValue))
        {

        }
        else
        {
            gripValue = -1;
        }

        if(DebugMode == true)
        {
            Debug.Log("Trigger: " + triggerValue);
            Debug.Log("Grip: " + gripValue);
        }

        controller.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primary2DAxisTouch);
        controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primary2DAxisPress);
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisTouch == true)
        {
            if(DebugMode == true)
            {
                Debug.Log("primary2DAxis: " + primary2DAxisValue);
            }
        }

        if (primary2DAxisPress == true && primary2DAxisPressed == false)
        {
            primary2DAxisPressed = true;
            if (DebugMode == true)
            {
                Debug.Log("primary2DAxisPressed: " + primary2DAxisValue);
            }
        }
        else if (primary2DAxisPress == false && primary2DAxisPressed == true)
        {
            primary2DAxisPressed = false;
        }
    }
}
