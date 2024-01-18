using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : EnemyBase
{
    [Header("파워업 아이템을 주는 적 정보")]

    // 등장하고 약간의 시간이 지난 후 일정시간동안 대기한다.
    // 죽을때 파워업 아이템을 떨군다.

    /// <summary>
    /// 등장 시간(처음 멈출때까지의 시간)
    /// </summary>
    public float appearTime = 0.5f;

    /// <summary>
    /// 대기 시간
    /// </summary>
    public float waitTime = 5.0f;

    /// <summary>
    /// 대기 시간 이후의 속도
    /// </summary>
    public float secondSpeed = 10.0f;

    /// <summary>
    /// 드랍할 아이템의 종류
    /// </summary>
    public PoolObjectType bonusType = PoolObjectType.PowerUp;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        StopAllCoroutines();
        StartCoroutine(AppearProcess());
    }

    IEnumerator AppearProcess()
    {
        yield return new WaitForSeconds(appearTime);
        moveSpeed = 0.0f;
        yield return new WaitForSeconds(waitTime);
        moveSpeed = secondSpeed;
    }

    protected override void OnDie()
    {
        Factory.Instance.GetObject(bonusType, transform.position);
        base.OnDie();
    }
}

// 실습
// 1. 움직일 때 뒤 스러스트 부분의 alpha가 깜박인다.
// 2. 대기할 때는 스러스트가 보이지 않는다.
