using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    public float thinkingTimeMin = 1.0f;
    public float thinkingTimeMax = 5.0f;


    protected override void Start()
    {
        base.Start();

        opponent = gameManager.UserPlayer;
    }
}

// 턴을 시작하면 랜덤한 시간(thinkingTimeMin~최대) 후에 자동 공격을 한다.(코루틴)
// 최대 = thinkingTimeMax와 TurnDuration 중 작은 것을 선택
// 