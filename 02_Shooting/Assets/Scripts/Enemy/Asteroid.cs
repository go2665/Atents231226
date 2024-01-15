using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : EnemyBase
{
    [Header("큰 운석 데이터")]
    // 이동 속도가 랜덤해야 한다.
    // 회전 속도도 랜덤이어야 한다.
    // 큰 운석은 수명을 가진다.(수명이 다되면 죽는다)
    //  수명도 랜덤이다.
    // 큰 운석은 죽을 때 작은 운석을 랜덤한 개수를 생성한다.
    //  모든 작은 운석은 서로 같은 사이각을 가진다.(작은 운석이 6개 생성 = 사이각 60도)
    //  criticalRate 확률로 작은 운석을 20개 생성한다.

    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;

    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;

    public float minLifeTime = 4.0f;
    public float maxLifeTime = 7.0f;

    public int minMiniCount = 3;
    public int maxMiniCount = 8;

    [Range(0f, 1f)]
    public float criticalRate = 0.05f;
    public int criticalMiniCount = 20;

    /// <summary>
    /// 회전 속도
    /// </summary>
    float rotateSpeed = 360.0f;

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector3 direction = Vector3.zero;

    /// <summary>
    /// 목적지를 이용해 방향을 결정하는 함수
    /// </summary>
    /// <param name="destination">목적지</param>
    public void SetDestination(Vector3 destination)
    {
        //Vector3 vec = destination - transform.position;
        //direction = vec.normalized;
        direction = (destination - transform.position).normalized;  
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(Time.deltaTime * moveSpeed * direction, Space.World);    // direction 방향으로 이동하기(월드기준)
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);    // 진행방향 표시
    }
}

// 실습
// 운석
//  1. 운석은 만들어졌을때 지정되는 도착점을 향해서 움직인다.
//  2. 운석은 계속 회전해야 한다.
//  3. 운석은 오브젝트 풀에서 관리되어야 한다.
//
// 스포너
//  1. 운석 생성용 스포너가 있어야 한다.
//  2. 운석을 생성하고 시작점과 도착점을 지정한다.
//  3. 도착점의 범위가 씬창에서 보여야 한다.



// 스포너에서 생성할 때 스포너의 자식이되는 문제 있음
