using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMisslie : EnemyBase
{
    [Header("추적 미사일 데이터")]
    /// <summary>
    /// 미사일의 유도 성능. 높일 수록 유도 성능이 좋아짐
    /// </summary>
    public float guidedPerformance = 1.5f;

    /// <summary>
    /// 추적 대상(플레이어)
    /// </summary>
    Transform target;

    /// <summary>
    /// 유도중인지 표시(true면 유도중, false면 유도 중지)
    /// </summary>
    bool onGuided = true;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        target = GameManager.Instance.Player.transform; // 활성화될때마다 플레이어 찾기
        onGuided = true;                                // 유도 켜기
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        if(onGuided)    // 유도 중이면
        {
            Vector3 dir = Vector3.left;
            if( target != null ) 
            {
                dir = target.position - transform.position; // 타겟으로 가는 방향 구하고
            }

            //transform.right = -dir;
            transform.right = -Vector3.Slerp(-transform.right, dir, deltaTime * guidedPerformance);   // 그쪽방향으로 회전 시키기            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(onGuided && collision.CompareTag("Player"))  // 유도 중일 때 플레이어가 트리거 영역안에 들어왔으면
        {
            onGuided = false;   // 유도 중지
        }
    }
}
