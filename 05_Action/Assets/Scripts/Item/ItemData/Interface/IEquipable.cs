using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    /// <summary>
    /// 이 아이템이 장착될 부위
    /// </summary>
    EquipType EquipType { get; }

    /// <summary>
    /// 아이템을 장착하는 함수
    /// </summary>
    /// <param name="target">장착받을 대상</param>
    /// <param name="slot">장착할 아이템이 들어있는 슬롯</param>
    void Equip(GameObject target, InvenSlot slot);

    /// <summary>
    /// 아이템을 장착 해제하는 함수
    /// </summary>
    /// <param name="target">장착 해제할 대상</param>
    /// <param name="slot">장착 해제할 아이템이 들어있는 슬롯</param>
    void UnEquip(GameObject target, InvenSlot slot);

    /// <summary>
    /// 상황에 맞게 장착 또는 장착 해제를 하는 함수
    /// </summary>
    /// <param name="target">대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    void ToggleEquip(GameObject target, InvenSlot slot);
}
