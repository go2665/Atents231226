using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - HealingPotion", menuName = "Scriptable Object/Item Data - HealingPotion", order = 4)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    /// <summary>
    /// 최대 HP에 비례해서 즉시 회복시켜주는 양
    /// </summary>
    public float healRatio = 0.3f;
    
    /// <summary>
    /// 일정 틱 동안 회복시켜주는 절대량
    /// </summary>
    public float tickRegen = 4.0f;
    public float tickInterval = 0.4f;
    public uint tickCount = 5;

    /// <summary>
    /// 힐링 포션을 사용하기 위한 함수
    /// </summary>
    /// <param name="target">힐링포션의 효과를 받을 대상</param>
    /// <returns>사용 성공 여부</returns>
    public bool Use(GameObject target)
    {
        bool result = false;

        IHealth health = target.GetComponent<IHealth>();
        if( health != null)
        {
            if( health.HP < health.MaxHP )
            {
                health.HP += health.MaxHP * healRatio;
                health.HealthRegenerateByTick(tickRegen, tickInterval, tickCount);
                result = true;
            }
            else
            {
                Debug.Log($"{target.name}의 HP가 가득 차 있습니다. 사용 불가");
            }
        }

        return result;
    }
}