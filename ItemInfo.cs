using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour, ISlotType
{
    [SerializeField] private SlotType[] slotTypes;

    public SlotType[] GetSlotTypes()
    {
        return slotTypes;
    }
}

public enum SlotType
{
    Head,
    Belt,
    Bag
}