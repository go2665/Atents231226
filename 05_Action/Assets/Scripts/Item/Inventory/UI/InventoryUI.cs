using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 보여줄 인벤토리
    /// </summary>
    Inventory inven;

    /// <summary>
    /// 인벤토리에 있는 slot UI들
    /// </summary>
    InvenSlotUI[] slotUIs;

    /// <summary>
    /// 임시 슬롯
    /// </summary>
    TempSlotUI tempSlotUI;

    /// <summary>
    /// 상세 정보창
    /// </summary>
    DetailInfoUI detail;

    /// <summary>
    /// 아이템 분리 창
    /// </summary>
    ItemDividerUI divider;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotUIs = child.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
        detail = GetComponentInChildren<DetailInfoUI>();
        divider = GetComponentInChildren<ItemDividerUI>(true);
        
    }

    /// <summary>
    /// 인벤토리 초기화용 함수
    /// </summary>
    /// <param name="playerInventory">이 UI가 표시할 인벤토리</param>
    public void InitializeInventory(Inventory playerInventory)
    {
        inven = playerInventory;    // 저장

        for(uint i=0; i<slotUIs.Length; i++)
        {
            slotUIs[i].InitializeSlot(inven[i]);    // 모든 인벤토리 슬롯 초기화
            slotUIs[i].onDragBegin += OnItemMoveBegin;      // 슬롯 델리게이트에 함수들 등록
            slotUIs[i].onDragEnd += OnItemMoveEnd;
            slotUIs[i].onClick += OnSlotClick;
            slotUIs[i].onPointerEnter += OnItemDetailOn;
            slotUIs[i].onPointerExit += OnItemDetailOff;
            slotUIs[i].onPointerMove += OnSlotPointerMove;
        }

        tempSlotUI.InitializeSlot(inven.TempSlot);  // 임시 슬롯 초기화

        divider.onOkClick += OnDividerOK;
        divider.onCancelClick += OnDividerCancel;
        divider.Close();
    }

    private void OnDividerOK(uint targetIndex, uint dividCount)
    {
        inven.DividItem(targetIndex, dividCount);
        tempSlotUI.Open();
    }

    private void OnDividerCancel()
    {
        detail.IsPause = false;
    }

    /// <summary>
    /// 드래그 시작했을 때 실행되는 함수
    /// </summary>
    /// <param name="index">드래그 시작한 위치에 있는 슬롯의 인덱스</param>
    private void OnItemMoveBegin(uint index)
    {
        detail.IsPause = true;
        inven.MoveItem(index, tempSlotUI.Index);        // 시작->임시로 아이템 옮기기  
        tempSlotUI.Open();                              // 임시 슬롯 열기
    }

    /// <summary>
    /// 드래그가 끝났을 때 실행되는 함수
    /// </summary>
    /// <param name="index">슬롯에서 드래그가 끝났으면 드래그가 끝난 슬롯의 인덱스, 아니면 드래그 시작한 슬롯의 인덱스</param>
    /// <param name="isSlotEnd">슬롯에서 드래그가 끝났으면 true, 아니면 false</param>
    private void OnItemMoveEnd(uint index, bool isSlotEnd)
    {
        //uint finalIndex = index;
        //if(!isSlotEnd)
        //{
        //    // 빈곳을 찾아서 따로 준다.
        //    if( inven.FindEmptySlot(out uint emptySlotIndex))
        //    {
        //        finalIndex = emptySlotIndex;
        //    }
        //    else
        //    {
        //        // 바닥에 드랍
        //        Debug.LogWarning("바닥에 아이템을 드랍해야 한다.");
        //        return;
        //    }
        //}

        //inven.MoveItem(tempSlotUI.Index, finalIndex);
        inven.MoveItem(tempSlotUI.Index, index);    // 임시 -> 도착으로 아이템 옮기기

        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            tempSlotUI.Close();                     // 임시 슬롯이 비면 닫는다.
        }

        detail.IsPause = false; // 퍼즈 풀고
        if(isSlotEnd)           // 슬롯에서 끝이 났으면 상세 정보창 다시 열기
        {
            detail.Open(inven[index].ItemData);
        }
    }

    /// <summary>
    /// 슬롯을 클릭했을 때 실행되는 함수
    /// </summary>
    /// <param name="index">클릭한 슬롯의 인덱스</param>
    private void OnSlotClick(uint index)
    {        
        if(tempSlotUI.InvenSlot.IsEmpty)
        {
            bool isShiftPress = (Keyboard.current.shiftKey.ReadValue() > 0);
            if(isShiftPress)
            {
                // 쉬프트가 눌려져 있는 상태 -> 아이템 분리 창 열기
                OnItemDividerOpen(index);
            }
            else
            {
                // 쉬프트가 안눌려진 상태 -> 아이템 사용 or 아이템 장비
            }

        }
        else
        {
            // 임시 슬롯에 아이템이 들어있으면
            OnItemMoveEnd(index, true);     // 클릭된 슬롯에 아이템 넣기(슬롯이 클릭되었을 때 실행되니 isSlotEnd는 true)
        }
    }

    /// <summary>
    /// 아이템 상세 정보창을 여는 함수
    /// </summary>
    /// <param name="index">상세 정보창에서 표시될 아이템이 들어있는 슬롯의 인덱스</param>
    private void OnItemDetailOn(uint index)
    {
        detail.Open(slotUIs[index].InvenSlot.ItemData); // 열기
    }

    /// <summary>
    /// 아이템 상세 정보창을 닫는 함수
    /// </summary>
    private void OnItemDetailOff()
    {
        detail.Close(); // 닫기
    }

    /// <summary>
    /// 슬롯안에서 마우스 커서가 움직였을 때 실행되는 함수
    /// </summary>
    /// <param name="screen">마우스 커서의 스크린 좌표</param>
    private void OnSlotPointerMove(Vector2 screen)
    {
        detail.MovePosition(screen);    // 움직이기
    }

    /// <summary>
    /// 아이템 분리 창 열기
    /// </summary>
    /// <param name="index">아이템을 분리할 슬롯의 인덱스</param>
    void OnItemDividerOpen(uint index)
    {
        InvenSlotUI target = slotUIs[index];
        divider.transform.position = target.transform.position + Vector3.up * 100;
        divider.Open(target.InvenSlot);
        detail.IsPause = true;
    }
}
