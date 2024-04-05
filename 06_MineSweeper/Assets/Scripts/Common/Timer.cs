using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // 게임 상태에 따라 시간을 측정하는 클래스

    /// <summary>
    /// 초 단위로 시간이 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<int> onTimeChange;

    private void Start()
    {
        GameManager manager = GameManager.Instance;
        manager.onGameReady += TimerReset;
        manager.onGamePlay += TimerReset;
        manager.onGamePlay += Play;
        manager.onGameGameClear += Stop;
        manager.onGameGameOver += Stop;
    }

    /// <summary>
    /// 타이머의 시간 측정을 시작하는 함수
    /// </summary>
    void Play()
    {

    }

    /// <summary>
    /// 타이머의 시간 측정을 정지하는 함수
    /// </summary>
    void Stop()
    {

    }

    /// <summary>
    /// 타이머의 데이터 초기화 하는 함수
    /// </summary>
    void TimerReset()
    {

    }

}
