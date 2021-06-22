using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public enum TwoHandRotationType
    {
        None,
        First,
        Second
    }

    [Header("GameDev Settings")]
    [Tooltip("Set this to debug helpful stuff")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private TwoHandRotationType twoHandRotationType;
    [SerializeField] private bool snapToSecondHand = true;

    [Space]
    [Header("Needed References")]
    [SerializeField] private XRSimpleInteractable[] secondHandGrabPoints;
    private XRBaseInteractor secondInteractor;
    private Quaternion attachIntitialRotation;
    private Quaternion initialRotationOffset;

    private void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    void Update()
    {

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor == true && selectingInteractor == true)
        {
            //Process rotation using both grab points
            if(snapToSecondHand == true)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }

        }

        base.ProcessInteractable(updatePhase);
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        ISocketInteractor socket = interactor.GetComponent<ISocketInteractor>();

        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        if(socket != null)
        {
            isAlreadyGrabbed = false;
        }

        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        if (debugMode == true)
        {
            Debug.Log("Second Hand Grab");
        }
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
    }
    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        if (debugMode == true)
        {
            Debug.Log("Second Hand Release");
        }
        secondInteractor = null;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if(debugMode == true)
        {
            Debug.Log("First Hand Grab");
        }
        base.OnSelectEntered(interactor);
        attachIntitialRotation = interactor.attachTransform.localRotation;
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        if (debugMode == true)
        {
            Debug.Log("First Hand Release");
        }
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachIntitialRotation;
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation = Quaternion.identity;

        switch (twoHandRotationType)
        {
            case TwoHandRotationType.None:
                targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
                break;
            case TwoHandRotationType.First:
                targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
                break;
            case TwoHandRotationType.Second:
                targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
                break;
        }

        return targetRotation;
    }
}
