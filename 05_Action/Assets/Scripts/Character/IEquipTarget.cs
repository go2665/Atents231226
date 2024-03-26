using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 아이템을 장비 가능한 클래스에 상속
/// </summary>
public interface IEquipTarget
{
    /// <summary>
    /// 장비 아이템 종류별 장착 여부 및 장비된 아이템의 슬롯 확인용
    /// </summary>
    /// <param name="part">확인할 부위</param>
    /// <returns>null이면 장비가 안되어 있다, null이 아니면 해당 슬롯에 있는 장비가 되어있다.</returns>
    InvenSlot this[EquipType part] { get; }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    void EquipItem(EquipType part, InvenSlot slot);

    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="part">장비 해제할 부위</param>
    void UnEquipItem(EquipType part);

    /// <summary>
    /// 장비될 아이템이 추가될 트랜스폼을 찾아주는 함수
    /// </summary>
    /// <param name="part">장비될 부위</param>
    /// <returns>장비될 부위의 부모 트랜스폼</returns>
    Transform GetEquipParentTransform(EquipType part);
}
