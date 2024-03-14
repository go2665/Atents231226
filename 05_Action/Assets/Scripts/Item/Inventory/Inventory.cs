using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 개념(메모리 상에서만 존재, UI 없음)
public class Inventory
{
    /// <summary>
    /// 인벤토리의 기본 슬롯 개수(6칸)
    /// </summary>
    const int Default_Inventory_Size = 6;

    /// <summary>
    /// 인벤토리의 슬롯들
    /// </summary>
    InvenSlot[] slots;

    /// <summary>
    /// 인벤토리 슬롯에 접근하기 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    /// <returns>슬롯</returns>
    public InvenSlot this[uint index] => slots[index];
    //public InvenSlot this[uint index] => (index != tempSlotIndex) ? slots[index] : tempSlot;  // 이렇게도 가능

    /// <summary>
    /// 인벤토리 슬롯의 개수
    /// </summary>
    int SlotCount => slots.Length;

    /// <summary>
    /// 임시 슬롯(드래그나 아이템 분리작업용)
    /// </summary>
    InvenSlot tempSlot;

    /// <summary>
    /// 임시 슬롯용 인덱스
    /// </summary>
    uint tempSlotIndex = 9999999;

    /// <summary>
    /// 임시 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenSlot TempSlot => tempSlot;

    /// <summary>
    /// 아이템 데이터 매니저(아이템의 종류별 정보를 모두 가지고 있다.)
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// 인벤토리를 가지고 있는 소유자
    /// </summary>
    Player owner;

    /// <summary>
    /// 소유자 확인용 프로퍼티
    /// </summary>
    public Player Owner => owner;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="owner">인벤토리의 소유자</param>
    /// <param name="size">인벤토리의 슬롯 개수</param>
    public Inventory(Player owner, uint size = Default_Inventory_Size)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        itemDataManager = GameManager.Instance.ItemData;    // 타이밍 조심
        this.owner = owner;
    }

    // 인벤토리가 해야할 일은?

    /// <summary>
    /// 인벤토리에 특정 아이템을 1개 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <returns>true면 추가 성공. false면 추가 실패</returns>
    public bool AddItem(ItemCode code)
    {
        for(int i = 0; i < SlotCount; i++)
        {
            if (AddItem(code, (uint)i))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에 특정 아이템을 1개 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <param name="slotIndex">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>true면 추가 성공. false면 추가 실패</returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if(IsValidIndex(slotIndex)) // 인덱스가 적절한지 확인
        {
            // 적절한 인덱스면
            ItemData data = itemDataManager[code];  // 데이터 가져오기
            InvenSlot slot = slots[slotIndex];      // 슬롯 가져오기
            if( slot.IsEmpty)
            {
                // 슬롯이 비었으면
                slot.AssignSlotItem(data);          // 그대로 아이템 설정
                result = true;
            }
            else
            {
                // 슬롯이 안비어있다.
                if(slot.ItemData == data)
                {
                    // 같은 종류의 아이템이면
                    result = slot.IncreaseSlotItem(out _);  // 아이템 증가 시도
                }
                else
                {
                    // 다른 종류의 아이템
                    Debug.Log($"아이템 추가 실패 : [{slotIndex}]번 슬롯에는 다른 아이템이 들어있습니다.");
                }
            }
        }
        else
        {
            // 인덱스가 잘못 들어오면 실패
            Debug.Log($"아이템 추가 실패 : [{slotIndex}]는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에서 아이템을 일정 개수만큼 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 감소시킬 슬롯의 인덱스</param>
    /// <param name="decreaseCount">감소시킬 개수</param>
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1) 
    { 
        if(IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            Debug.Log($"슬롯 아이템 감소 실패 : [{slotIndex}]는 없는 인덱스입니다.");
        }
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯을 비우는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 비울 슬롯</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"슬롯 아이템 삭제 실패 : [{slotIndex}]는 없는 인덱스입니다.");
        }
    }

    /// <summary>
    /// 인벤토리의 슬롯을 전부 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    /// <summary>
    /// 인벤토리의 from 슬롯에 있는 아이템을 to 위치로 옮기는 함수
    /// </summary>
    /// <param name="from">위치 변경 시작 인덱스</param>
    /// <param name="to">위치 변경 도착 인덱스</param>
    public void MoveItem(uint from, uint to)
    {
        // from지점과 to지점은 서로 다른 위치이고 모두 valid한 인덱스이어야 한다.
        if ( (from != to) && IsValidIndex(from) && IsValidIndex(to) )
        {
            InvenSlot fromSlot = (from == tempSlotIndex) ? TempSlot : slots[from];

            if( !fromSlot.IsEmpty)
            {
                // from에 아이템이 있다
                InvenSlot toSlot = (to == tempSlotIndex) ? TempSlot : slots[to];

                if( fromSlot.ItemData == toSlot.ItemData )
                {
                    // 같은 종류의 아이템 => to에 채울 수 있는데까지 채운다. to에 넘어간 만큼 from을 감소시킨다.
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // from이 가진 개수만큼 tro 추가
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 to로 넘어간 개수만큼만 감소   
                    // 5/10 -> 7/10으로 옮긴 상황이라고 가정하면
                    // over == 2
                    // (5-2) 만큼 감소

                    Debug.Log($"[{from}]번 슬롯에서 [{to}]번 슬롯으로 아이템 합치기");
                }
                else
                {
                    // 다른 종류의 아이템(or 비어있다) => 서로 스왑
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    bool tempEquip = fromSlot.IsEquipped;

                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount, toSlot.IsEquipped);
                    toSlot.AssignSlotItem(tempData, tempCount, tempEquip);
                    Debug.Log($"[{from}]번 슬롯과 [{to}]번 슬롯을 서로 아이템 교체");
                }
            }
        }
    }

    // 아이템 나누기
    // 아이템 정렬

    /// <summary>
    /// 슬롯 인덱스가 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스, false면 잘못된 인덱스</returns>
    bool IsValidIndex(uint index)
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }


#if UNITY_EDITOR
    public void Test_InventoryPrint()
    {
        // 출력 예시
        // [ 루비(1/10), 사파이어(2/3), (빈칸), 에메랄드(3/5), (빈칸), (빈칸) ]
    }
#endif
}
