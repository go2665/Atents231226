using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : TurretBase
{   
    /// <summary>
    /// 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 터렛의 머리가 돌아가는 속도
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 터렛이 총알을 발사를 시작하는 좌우발사각(10이면 +-10도)
    /// </summary>
    public float fireAngle = 10.0f;

    
    
    /// <summary>
    /// 시야범위 체크용 트리거
    /// </summary>
    SphereCollider sightTrigger;

    /// <summary>
    /// 내 시야 범위 안에 들어온 플레이어
    /// </summary>
    Player target;

    /// <summary>
    /// 발사 중인지 아닌지 표시하는 변수(true면 발사 중)
    /// </summary>
    bool isFiring = false;

    protected override void Awake()
    {
        // base : 부모 클래스의 인스턴스에 접근하는 참조
        base.Awake();   
        sightTrigger = GetComponent<SphereCollider>();        
    }

    private void Start()
    {
        sightTrigger.radius = sightRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == GameManager.Instance.Player.transform)
        {
            target = GameManager.Instance.Player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == GameManager.Instance.Player.transform)
        {
            target = null;
        }
    }

    private void Update()
    {
        LookTargetAndAttack();
    }

    private void LookTargetAndAttack()
    {
        bool isStartFire = false;
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0.0f;

            //barrelBody.forward = dir; // 즉시 바라보기
            barrelBody.rotation = Quaternion.Slerp(
                barrelBody.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * turnSpeed);

            //Vector3.SignedAngle : 두 벡터의 사이각을 구하는데 방향을 고려하여 계산한다.            
            float angle = Vector3.Angle(barrelBody.forward, dir);
            if( angle < fireAngle )
            {
                isStartFire = true; // 발사 결정
            }
        }
        
        if(isStartFire)     // 발사해야 하는 상황인지 확인
        {
            StartFire();    // 발사 시작
        }
        else
        {
            StopFire();     // 발사 정지
        }
    }

    /// <summary>
    /// 총알을 발사하기 시작(중복 실행 없음)
    /// </summary>
    void StartFire()
    {
        if(!isFiring)
        {
            StartCoroutine(fireCoroutine);
            isFiring = true;
        }
    }

    /// <summary>
    /// 총알 발사를 정지
    /// </summary>
    void StopFire()
    {
        if(isFiring)
        {
            StopCoroutine(fireCoroutine);
            isFiring = false;
        }
    }


#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // sightRange 범위 그리기
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 3.0f);

        if(barrelBody == null)
        {
            barrelBody = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + barrelBody.forward * sightRange;

        // 중심 선 그리기
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        // 시야 각 내부 그리기
        Handles.color = Color.green;

        // 평소(녹색), 플레이어 쪽으로 머리 돌리는 중(주황색), 플레이어를 공격하는 상황(빨간색)

        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * barrelBody.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * barrelBody.forward;

        to = transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to, 2.0f);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to, 2.0f);
    }
#endif
}
