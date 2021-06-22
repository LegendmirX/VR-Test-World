using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    [SerializeField] private XRController rightTeleRay;
    [SerializeField] private InputHelpers.Button teleportActivationButton;
    [SerializeField] private float activationThreshold = 0.1f;

    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private XRRayInteractor leftRayInteractor;

    public bool CurrentlyHoldingObject { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;

        if(rightTeleRay != null)
        {

            bool isRightReyInteractorHovering = rightRayInteractor.TryGetHitInfo(out pos, out norm, out index, out validTarget);

            rightTeleRay.gameObject.SetActive(CurrentlyHoldingObject == false && CheckIfActivated(rightTeleRay) && isRightReyInteractorHovering == false);
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
