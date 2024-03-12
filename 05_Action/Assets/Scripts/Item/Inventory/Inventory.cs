using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 개념(메모리 상에서만 존재, UI 없음)
public class Inventory
{
    const int Default_Inventory_Size = 6;

    InvenSlot[] slots;

    public InvenSlot this[uint index] => slots[index];
    //public InvenSlot this[uint index] => (index != tempSlotIndex) ? slots[index] : tempSlot;  // 이렇게도 가능

    int SlotCount => slots.Length;

    InvenSlot tempSlot;
    uint tempSlotIndex = 9999999;
    public InvenSlot TempSlot => tempSlot;

    ItemDataManager itemDataManager;

    Player owner;
    public Player Owner => owner;
}
