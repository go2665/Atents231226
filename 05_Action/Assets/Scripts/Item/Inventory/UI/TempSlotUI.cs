using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUI_Base
{
    InvenTempSlot tempSlot;

    public uint FromIndex => tempSlot.FromIndex;
    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        
    }

    public override void InitializeSlot(InvenSlot slot)
    {
        base.InitializeSlot(slot);
        tempSlot = slot as InvenTempSlot;
        Close();
    }

    public void Open()
    {
        transform.position = Mouse.current.position.ReadValue();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetFromIndex(uint index)
    {
        tempSlot.SetFromIndex(index);
    }
}
