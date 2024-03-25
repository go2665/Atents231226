using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 맴버 함수에 대한 선언만 가능하다.
public interface IMana
{
    /// <summary>
    /// MP 확인 및 설정용 프로퍼티
    /// </summary>
    float MP { get; set; }

    /// <summary>
    /// 최대 HP확인용 프로퍼티
    /// </summary>
    float MaxMP { get; }

    /// <summary>
    /// HP가 변경될 때마다 실행될 델리게이트(float:비율)용 프로퍼티
    /// </summary>
    Action<float> onManaChange { get; set; }

    /// <summary>
    /// 체력을 지속적으로 증가시켜 주는 함수. 초당 totalRegen/duration 만큼 회복
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복되는데 걸리는 시간</param>
    void ManaRegenerate(float totalRegen, float duration);

    /// <summary>
    /// 체력을 틱단위로 회복시켜 주는 함수. 
    /// 전체 회복량 = tickRegen * totalTickCount. 전체 회복 시간 = tickInterval * totalTickCount
    /// </summary>
    /// <param name="tickRegen">틱 당 회복량</param>
    /// <param name="tickInterval">틱 간의 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 수</param>
    void ManaRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount);

}
