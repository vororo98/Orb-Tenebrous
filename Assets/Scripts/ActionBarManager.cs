using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarManager : MonoBehaviour
{
    [SerializeField] private List<ActionSlot> slots;

    public agentType getSlot(int index)
    {
        return slots[index].getEquippedType();
    }

    public void DeleteAllEquipped()
    {
        foreach (ActionSlot slot in slots)
        {
            slot.DeleteEquipped();
        }
    }
}
