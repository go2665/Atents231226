using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Boss : EnemyBase
{
    [Header("보스 데이터")]
    /// <summary>
    /// 보스의 총알
    /// </summary>
    public PoolObjectType bullet = PoolObjectType.EnemyBossBullet;

    /// <summary>
    /// 보스의 미사일
    /// </summary>
    public PoolObjectType misslie = PoolObjectType.EnemyBossMisslie;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float bulletInterval = 1.0f;

    /// <summary>
    /// 보스 활동영역(최소, 월드좌표)
    /// </summary>
    public Vector2 areaMin = new Vector2(2, -3);

    /// <summary>
    /// 보스 활동영역(최대, 월드좌표)
    /// </summary>
    public Vector2 areaMax = new Vector2(7, 3);

    /// <summary>
    /// 미사일 일제사격 회수
    /// </summary>
    public int barrageCount = 3;

    /// <summary>
    /// 총알 발사 위치1 
    /// </summary>
    Transform fire1;

    /// <summary>
    /// 총알 발사 위치2
    /// </summary>
    Transform fire2;

    /// <summary>
    /// 미사일 발사 위치
    /// </summary>
    Transform fire3;

    /// <summary>
    /// 보스의 이동 방향
    /// </summary>
    Vector3 moveDirection = Vector3.left;

    private void Awake()
    {
        Transform fireTransforms = transform.GetChild(1);
        fire1 = fireTransforms.GetChild(0);
        fire2 = fireTransforms.GetChild(1);
        fire3 = fireTransforms.GetChild(2);
    }

    /// <summary>
    /// 보스의 스폰이 완료된 후에 마지막으로 실행되는 함수
    /// </summary>
    public void OnSpawn()
    {
        StopAllCoroutines();    // 꺼낼 때 실행되던 모든 코루틴 정지 
        StartCoroutine(MovePaternProcess());
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * moveDirection); // 항상 moveDirection방향으로 움직인다.
    }

    /// <summary>
    /// 보스 움직임 패턴용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator MovePaternProcess()
    {
        moveDirection = Vector3.left;           // 처음에는 왼쪽으로 움직인다.

        float middleX = (areaMax.x - areaMin.x) * 0.5f + areaMin.x; // area의 가운데 구하기
        while(transform.position.x > middleX)   // 중간지점에 도달할 때까지 계속 왼쪽으로 진행
        {
            yield return null;
        }

        // 중간 지점에 도착
        StartCoroutine(FireBullet());   // 총알 발사하기 시작
        ChangeDirection();              // 아래로 움직이기 시작
        
        // 위아래 반복하기
        while(true)
        {
            // 영역 최대 높이보다 올라가거나, 최소 높이보다 낮아지면 방향 전환
            if(transform.position.y > areaMax.y || transform.position.y < areaMin.y)
            {
                ChangeDirection();              // 방향 전환
                StartCoroutine(FireMisslie());  // 방향 전환 할 때마다 미사일 쏘기
            }
            yield return null;
        }
    }

    /// <summary>
    /// 보스의 위 아래 방향을 변경하는 함수
    /// </summary>
    void ChangeDirection()
    {
        Vector3 target = new Vector3();
        target.x = Random.Range(areaMin.x, areaMax.x);                  // x 위치는 최소~최대 사이
        target.y = (transform.position.y > 0) ? areaMin.y : areaMax.y;  // y 위치는 올라가던 중이면 최소, 내려가던 중이면 최대
        //Debug.Log(target);

        moveDirection = (target - transform.position).normalized;       // 방향 수정
    }

    private void OnDrawGizmosSelected()
    {
        // 보스가 움직이는 영역 그리기
        Gizmos.color = Color.blue;

        Vector3 p0 = new(areaMin.x, areaMin.y);
        Vector3 p1 = new(areaMax.x, areaMin.y);
        Vector3 p2 = new(areaMax.x, areaMax.y);
        Vector3 p3 = new(areaMin.x, areaMax.y);
            
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }

    /// <summary>
    /// 총알 계속 발사하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireBullet()
    {
        while(true)
        {
            Factory.Instance.GetObject(PoolObjectType.EnemyBossBullet, fire1.position);
            Factory.Instance.GetObject(PoolObjectType.EnemyBossBullet, fire2.position);

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    /// <summary>
    /// 미사일을 빠르게 연사하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireMisslie()
    {
        for(int i=0;i<barrageCount;i++)
        {
            Factory.Instance.GetObject(PoolObjectType.EnemyBossMisslie, fire3.position);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
