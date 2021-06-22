using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventorySnapZone : XRSocketInteractor, ISocketInteractor
{
    [Tooltip("What type of snap point is this?")]
    [SerializeField] private SlotType[] acceptedTypes;
    private List<Collider> itemCols;

    protected override void OnSelectEntering(XRBaseInteractable interactable)
    {
        Debug.Log("OnSocket Entering");
        //ISlotType slotType = interactable.transform.GetComponent<ISlotType>();
        //bool canEnter = false;
        //foreach(SlotType slot in slotType.GetSlotTypes())
        //{
        //    foreach(SlotType acceptedSlot in acceptedTypes)
        //    {
        //        if(slot == acceptedSlot)
        //        {
        //            canEnter = true;
        //        }
        //    }
        //}

        //if(canEnter == false)
        //{
        //    return;
        //}

        base.OnSelectEntering(interactable);
    }
    protected override void OnSelectEntered(XRBaseInteractable interactable)
    {
        Debug.Log("OnSocket Entered");
        itemCols = interactable.colliders;
        if(itemCols != null && itemCols.Count > 0)
        {
            foreach (Collider itemCol in itemCols)
            {
                itemCol.enabled = false;
            }
        }
        base.OnSelectEntered(interactable);
    }
    protected override void OnSelectExiting(XRBaseInteractable interactable)
    {
        Debug.Log("OnSocket Exiting");
        base.OnSelectExiting(interactable);
    }
    protected override void OnSelectExited(XRBaseInteractable interactable)
    {
        Debug.Log("OnSocket Exit");
        if (itemCols != null && itemCols.Count > 0)
        {
            foreach (Collider itemCol in itemCols)
            {
                itemCol.enabled = true;
            }
        }
        itemCols = null;
        base.OnSelectExited(interactable);
    }
}