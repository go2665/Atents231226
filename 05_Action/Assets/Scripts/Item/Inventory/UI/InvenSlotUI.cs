using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotUI : SlotUI_Base
{
    /// <summary>
    /// 장비여부 표시용 텍스트
    /// </summary>
    TextMeshProUGUI equipText;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        equipText = child.GetComponent<TextMeshProUGUI>();
    }

    protected override void OnRefresh()
    {
        // 화면 갱신할 때 장비 여부에 따라 장비 글자 색 변경
        if(InvenSlot.IsEquipped)
        {
            equipText.color = Color.red;        // 장비했을때는 빨간색
        }
        else
        {
            equipText.color = Color.clear;      // 장비하지 않았을 때는 투명
        }
    }
}
