using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // HP관련 ------------------------------------------------------------------------------------------
    [SerializeField]    // 테스트용
    float hp = 30.0f;
    [SerializeField]    // 성능을 추구한다면 비권장. 객체지향적으로는 더 올바름
    float maxHP = 30.0f;

    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                State = BehaviorState.Dead;
            }
        }
    }

    // 상태 관련 ------------------------------------------------------------------------------------------
    public enum BehaviorState : byte
    {
        Wander = 0, // 배회상태. 주변을 왔다갔다한다.
        Chase,      // 추적상태. 플레이어가 마지막으로 목격된 장소를 향해 계속 이동한다.
        Find,       // 탐색상태. 추적 도중에 플레이어가 시야에서 사라지면 두리번 거리며 주변을 찾는다.
        Attack,     // 공격상태. 플레이어가 일정범위안에 들어오면 일정 주기로 공격을 한다.
        Dead        // 사망상태. 죽는다.(일정 시간 후에 재생성)
    }

    BehaviorState state = BehaviorState.Dead;
    BehaviorState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                OnStateExit(value);
                state = value;
                OnStateEnter(value);
            }
        }
    }

    /// <summary>
    /// 각 상태가 되었을때 상태별 업데이트 함수를 저장하는 델리게이트(함수포인터의 역할)
    /// </summary>
    Action onUpdate = null;

    void OnStateEnter(BehaviorState newState)
    {

    }

    void OnStateExit(BehaviorState newState)
    {

    }

    // 이동 관련 ----------------------------------------------------------------------------------------
}
