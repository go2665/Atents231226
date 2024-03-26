using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("장비 아이템 데이터")]
    /// <summary>
    /// 아이템을 장비 했을 때 플레이어 모델에 추가될 프리팹
    /// </summary>
    public GameObject equipPrefab;

    /// <summary>
    /// 아이템 장비될 위치를 알려주는 프로퍼티
    /// </summary>
    public virtual EquipType EquipType => EquipType.Weapon;

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    public void Equip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if(equipTarget != null)
        {
            equipTarget.EquipItem(EquipType, slot);
        }
    }

    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="part">장비 해제할 부위</param>
    public void UnEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if (equipTarget != null)
        {
            equipTarget.UnEquipItem(EquipType);
        }
    }

    /// <summary>
    /// 장비될 아이템이 추가될 트랜스폼을 찾아주는 함수
    /// </summary>
    /// <param name="part">장비될 부위</param>
    /// <returns>장비될 부위의 부모 트랜스폼</returns>
    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
        if(equipTarget != null)
        {
            InvenSlot oldSlot = equipTarget[EquipType];
            if (oldSlot == null)
            {
                // 아무것도 장비되어있지 않다.
                Equip(target, slot);    // 입력 받은 것 장비
            }
            else
            {
                // 무언가가 장비되어 있다.
                UnEquip(target, oldSlot);   // 이전 것 장비 해제
                if( oldSlot != slot )
                {
                    Equip(target, slot);    // 다른 슬롯에 있는 장비를 클릭한 것이었으면 그 슬롯에 있는 것 장비
                }
            }
        }
    }
}
