using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : EnemyBase
{
    [Header("큰 운석 데이터")]

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
    /// 원래 점수(자폭했을 때 점수를 안주기 위해 필요)
    /// </summary>
    int originalScore;

    private void Awake()
    {
        originalScore = score;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        score = originalScore;

        StartCoroutine(SelfCrush());
    }

    /// <summary>
    /// 목적지를 이용해 방향을 결정하는 함수
    /// </summary>
    /// <param name="destination">목적지</param>
    public void SetDestination(Vector3 destination)
    {
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

    IEnumerator SelfCrush()
    {
        float lifeTime = Random.Range(minLifeTime, maxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        score = 0;
        OnDie();
    }

    protected override void OnDie()
    {
        int count = criticalMiniCount;

        if( Random.value > criticalRate )
        {
            count = Random.Range(minMiniCount, maxMiniCount);
        }

        float angle = 360.0f / count;
        float startAngle = Random.Range(0, 360.0f);
        for(int i=0;i<count; i++)
        {
            Factory.Instance.GetAsteroidMini(transform.position, startAngle + angle * i);
        }

        base.OnDie();
    }
}