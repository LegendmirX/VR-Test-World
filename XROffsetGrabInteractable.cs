using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 attachStartPos;
    private Quaternion attachStartRot;

    // Start is called before the first frame update
    void Start()
    {
        if (!attachTransform) //Make pivot if needed
        {
            GameObject grab = new GameObject("Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        attachStartPos = attachTransform.localPosition;
        attachStartRot = attachTransform.localRotation;
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        if(interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = attachStartPos;
            attachTransform.localRotation = attachStartRot;
        }

        base.OnSelectEntering(interactor);
    }
}
