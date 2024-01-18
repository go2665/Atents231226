using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : EnemyBase
{
    [Header("커브를 도는 적 데이터")]
    /// <summary>
    /// 회전하는 속도
    /// </summary>
    public float rotateSpeed = 10.0f;

    /// <summary>
    /// 회전 방향(1이면 반시계방향, -1이면 시계방향)
    /// </summary>
    float curveDirection = 1.0f;

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        transform.Rotate(deltaTime* rotateSpeed * curveDirection * Vector3.forward);    // 회전 추가
    }

    /// <summary>
    /// 현재 높이에 따라 회전방향을 갱신하는 함수
    /// </summary>
    public void RefreashRotateDirection()
    {
        if(transform.position.y < 0)
        {
            // 화면 가운데보다 아래쪽이면 우회전
            curveDirection = -1.0f;
        }
        else
        {
            // 화면 가운데보다 위쪽으면 좌회전
            curveDirection = 1.0f;
        }
    }
}

