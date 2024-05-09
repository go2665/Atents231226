using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    /// <summary>
    /// 자동 공격 최소 대기 시간
    /// </summary>
    public float thinkingTimeMin = 1.0f;

    /// <summary>
    /// 자동 공격 최대 대기 시간
    /// </summary>
    public float thinkingTimeMax = 5.0f;


    protected override void Start()
    {
        base.Start();

        opponent = gameManager.UserPlayer;  // 상대방 설정하기

        thinkingTimeMax = Mathf.Min(thinkingTimeMax, turnManager.TurnDuration); // 최대 대기 시간 설정(TurnDuration보다 작거나 같아야 한다)
    }

    /// <summary>
    /// 턴이 시작될 때 실행되는 함수
    /// </summary>
    /// <param name="_">사용안함</param>
    protected override void OnPlayerTurnStart(int _)
    {
        base.OnPlayerTurnStart(_);

        float delay = Random.Range(thinkingTimeMin, thinkingTimeMax);   // 자동 공격 딜레이 설정
        StartCoroutine(AutoStart(delay));
    }

    /// <summary>
    /// 턴이 종료될 때 실행되는 함수
    /// </summary>
    protected override void OnPlayerTurnEnd()
    {
        StopAllCoroutines();    // 타임 아웃일 때 코루틴 실행되는 것 방지
        base.OnPlayerTurnEnd();
    }

    /// <summary>
    /// 딜레이 후 자동 공격하는 코루틴
    /// </summary>
    /// <param name="delay">대기 시간</param>
    /// <returns></returns>
    IEnumerator AutoStart(float delay)
    {
        yield return new WaitForSeconds(delay); // delay만큼 기다린 후
        AutoAttack();                           // 자동 공격
    }

    /// <summary>
    /// 패배 했을 때 실행되는 함수
    /// </summary>
    protected override void OnDefeat()
    {
        StopAllCoroutines();    // 패배 후 공격 방지
        base.OnDefeat();       
    }
}
