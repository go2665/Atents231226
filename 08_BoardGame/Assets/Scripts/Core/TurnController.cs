using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    /// <summary>
    /// 현재 턴 번호(몇번째 턴인지)
    /// </summary>
    int turnNumber = 1;

    /// <summary>
    /// 턴 타임아웃되는데 걸리는 시간
    /// </summary>
    public float turnDuration = 5.0f;

    /// <summary>
    /// 타임 아웃 시간 확인용 프로퍼티
    /// </summary>
    public float TurnDuration => turnDuration;

    /// <summary>
    /// 이번 턴이 타임아웃될 때 까지 남아있는 시간
    /// </summary>
    float turnRemainTime = 0.0f;

    /// <summary>
    /// 타임아웃이 활성화되어 있는지 표시(false면 타임아웃이 일어나지 않는다)
    /// </summary>
    public bool timeOutEnable = true;

    /// <summary>
    /// 현재 턴 진행상황 표시용 enum
    /// </summary>
    enum TurnProcessState
    {
        None = 0,   // 행동을 완료한 사람이 없음
        One,        // 한명만 행동을 완료함
        Both        // 둘 다 행동을 완료함
    }

    /// <summary>
    /// 현재 턴 진행상황
    /// </summary>
    TurnProcessState state = TurnProcessState.None;

    /// <summary>
    /// 턴이 진행될지 여부(true면 턴이 진행되고 false면 턴이 진행되지 않는다)
    /// </summary>
    bool isTurnEnable = true;

    /// <summary>
    /// 턴이 시작되었음을 알리는 델리게이트(int:시작된 턴 번호)
    /// </summary>
    public Action<int> onTurnStart;

    /// <summary>
    /// 턴이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onTurnEnd;

    /// <summary>
    /// 턴 종료 처리 중인지 확인하는 변수
    /// </summary>
    bool isEndProcess = false;

    /// <summary>
    /// 씬이 시작될 떄 초기화
    /// </summary>
    public void OnInitialize(PlayerBase user, PlayerBase enemy)
    {
        turnNumber = 0;     // OnTurnStart에서 turnNumber를 증가 시키기 때문에 0에서 시작

        if (!timeOutEnable)                 // 타임 아웃을 껏으면
        {
            turnDuration = float.MaxValue;  // turnDuration을 매우 길게 잡기
        }
        turnRemainTime = TurnDuration;      // 턴 남은 시간을 turnDuration으로 설정

        state = TurnProcessState.None;      // 턴 진행 상태 초기화
        isTurnEnable = true;                // 턴 켜기

        onTurnStart = null;                 // 델리게이트 초기화        
        onTurnEnd = null;

        if(user != null)                    // user가 있으면 행동이 끝났거나 패배했을 때 실행될 함수 연결
        {
            user.onActionEnd += PlayerTurnEnd;
            user.onDefeat += TurnManagerStop;
        }

        if(enemy != null)                   // enemy가 있으면 행동이 끝났거나 패배했을 때 실행될 함수 연결
        {
            enemy.onActionEnd += PlayerTurnEnd;
            enemy.onDefeat += TurnManagerStop;
        }

        OnTurnStart();                      // 턴 시작
    }

    private void Update()
    {
        turnRemainTime -= Time.deltaTime;
        if(isTurnEnable && turnRemainTime < 0.0f )  // 턴 매니저가 작동 중이면, 타임아웃이 되었는지 체크
        {
            OnTurnEnd();    // 타임 아웃이 되면 턴 종료 처리
        }
    }

    /// <summary>
    /// 턴 시작 처리용 함수
    /// </summary>
    void OnTurnStart()
    {
        if(isTurnEnable)    // 턴 매니저가 작동 중이면
        {
            turnNumber++;                       // 턴 숫자 증가
            Debug.Log($"{turnNumber}턴 시작");
            state = TurnProcessState.None;      // 상태 초기화
            turnRemainTime = TurnDuration;      // 타임 아웃용 시간 리셋

            onTurnStart?.Invoke(turnNumber);    // 턴이 시작되었음을 알림
        }
    }

    /// <summary>
    /// 턴 종료 처리용 함수
    /// </summary>
    void OnTurnEnd()
    {
        if(isTurnEnable)    // 턴 매니저가 작동 중이면
        {
            isEndProcess = true;    // 종료 처리 중이라고 표시
            onTurnEnd?.Invoke();    // 턴이 종료되었다고 알림
            Debug.Log($"{turnNumber}턴 종료");

            isEndProcess = false;   // 종료 처리가 끝났다고 표시
            OnTurnStart();          // 다음 턴 시작
        }
    }

    /// <summary>
    /// 플레이어가 행동을 완료하면 실행되는 함수
    /// </summary>
    void PlayerTurnEnd()
    {
        if(!isEndProcess)   // 종료 처리 중이 아니면
        {
            state++;        // 상태를 다음 상태로 변경
            if(state >= TurnProcessState.Both)  // Both가 되면
            {
                OnTurnEnd();                    // 턴 종료
            }
        }
    }

    /// <summary>
    /// 턴 매니저를 정지 시키는 함수
    /// </summary>
    /// <param name="_"></param>
    void TurnManagerStop(PlayerBase _)
    {
        isTurnEnable = false;
    }
}
