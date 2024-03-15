using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    InvenSlot invenSlot;

    public InvenSlot InvenSlot => invenSlot;

    Image itemIcon;
    TextMeshProUGUI itemCount;

    public uint Index => invenSlot.Index;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemCount = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯을 초기화 하는 함수(=InvenSlot과 InvenSlotUI를 연결)
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InvenSlot slot)
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;
        Refresh();
    }

    private void Refresh()
    {
        if (InvenSlot.IsEmpty)
        {
            itemIcon.color = Color.clear;
            itemIcon.sprite = null;
            itemCount.text = string.Empty;
        }
        else
        {
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;
            itemIcon.color = Color.white;
            itemCount.text = InvenSlot.ItemCount.ToString();
        }
        OnRefresh();
    }

    protected virtual void OnRefresh()
    {
        // 장비 여부 표시 갱신용
    }
}
