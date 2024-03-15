using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    InvenSlotUI[] slotUIs;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotUIs = child.GetComponentsInChildren<InvenSlotUI>();
    }

    public void InitializeInventory(Inventory playerInventory)
    {
        inven = playerInventory;

        for(uint i=0; i<slotUIs.Length; i++)
        {
            slotUIs[i].InitializeSlot(inven[i]);
        }
    }
}
