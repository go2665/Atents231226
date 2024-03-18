using System;
using System.Collections.Generic;

public class InvenTempSlot : InvenSlot
{
    const uint NotSet = uint.MaxValue;

    uint fromIndex = NotSet;

    public uint FromIndex => fromIndex;

    public InvenTempSlot(uint index) : base(index)
    {
        fromIndex = NotSet;
    }

    //public override void AssignSlotItem(ItemData data, uint count = 1, bool isEquipped = false)
    //{
    //    base.AssignSlotItem(data, count, isEquipped);
    //    //fromIndex = from ?? NotSet; // ?? : null이 아니면 가지고 있는 값, null이면 뒤에 있는 값
    //}

    public override void ClearSlotItem()
    {
        base.ClearSlotItem();
        fromIndex = NotSet;
    }

    public void SetFromIndex(uint index)
    {
        fromIndex = index;
    }
}
