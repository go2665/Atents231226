using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretTrace : MonoBehaviour
{
    /// <summary>
    /// 터렛이 발사할 총알 종류
    /// </summary>
    public PoolObjectType bulletType = PoolObjectType.Bullet;
    
    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 1.0f;

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
    /// 총몸의 트랜스폼
    /// </summary>
    Transform barrelBody;

    /// <summary>
    /// 총알 발사 위치 설정용 트랜스폼
    /// </summary>
    Transform fireTransform;

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

    /// <summary>
    /// 총알을 주기적으로 발사하는 코루틴
    /// </summary>
    IEnumerator fireCoroutine;

    private void Awake()
    {
        sightTrigger = GetComponent<SphereCollider>();
        barrelBody = transform.GetChild(2);
        fireTransform = barrelBody.GetChild(1);
        fireCoroutine = PeriodFire();
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

    IEnumerator PeriodFire()
    {
        while (true)
        {
            Factory.Instance.GetObject(bulletType, fireTransform.position, fireTransform.rotation.eulerAngles);
            yield return new WaitForSeconds(fireInterval);
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
    private void OnDrawGizmos()
    {
        //Gizmos
        Handles.DrawWireDisc(transform.position, transform.up, sightRange, 3.0f);

        if(barrelBody == null)
        {
            barrelBody = transform.GetChild(2);
        }

        Vector3 from = transform.position;
        Vector3 to = transform.position + barrelBody.forward * sightRange;
        Handles.color = Color.yellow;
        Handles.DrawDottedLine(from, to, 2.0f);

        Handles.color = Color.red;
        Vector3 dir1 = Quaternion.AngleAxis(-fireAngle, transform.up) * barrelBody.forward;
        Vector3 dir2 = Quaternion.AngleAxis(fireAngle, transform.up) * barrelBody.forward;

        to = transform.position + dir1 * sightRange;
        Handles.DrawLine(from, to, 2.0f);
        to = transform.position + dir2 * sightRange;
        Handles.DrawLine(from, to, 2.0f);
    }
#endif
}

// 실습 : 추적용 터렛 만들기
// 1. 플레이어가 터렛으로 부터 일정 거리안에 있으면 플레이어쪽으로 BarrelBody가 회전한다(플레이어를 바라보기, y축으로만 회전)
// 2. 플레이어가 터렛의 발사각 안에 있으면 총알을 주기적으로 발사한다.
// 3. 플레이어가 터렛의 발사각 안에 없으면 총알 발사를 정지한다.
// 4. Gizmos를 통해 시야 범위와 발사각을 그린다.(Handles 추천)