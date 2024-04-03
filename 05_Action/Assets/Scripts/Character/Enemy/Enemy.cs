using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : RecycleObject, IBattler, IHealth
{
    /// <summary>
    /// 적이 가질 수 있는 상태의 종류
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,   // 대기
        Patrol,     // 순찰
        Chase,      // 추적
        Attack,     // 공격
        Dead        // 사망
    }

    /// <summary>
    /// 적의 현재 상태
    /// </summary>
    EnemyState state = EnemyState.Patrol;

    /// <summary>
    /// 상태를 설정하고 확인하는 프로퍼티
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)  // 상태에 진입할 때 할 일들 처리
                {
                    case EnemyState.Wait:
                        // 일정 시간 대기
                        agent.isStopped = true;         // agent 정지
                        agent.velocity = Vector3.zero;  // agent에 남아있던 운동량 제거
                        animator.SetTrigger("Stop");    // 애니메이션 정지
                        WaitTimer = waitTime;           // 기다려야 하는 시간 초기화
                        onStateUpdate = Update_Wait;    // 대기 상태용 업데이트 함수 설정
                        break;                        
                    case EnemyState.Patrol:
                        // Debug.Log("패트롤 상태");
                        agent.isStopped = false;        // agent 다시 켜기
                        agent.SetDestination(waypoints.NextTarget);  // 목적지 지정(웨이포인트 지점)
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Patrol;
                        break;                        
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Chase;
                        break;                        
                    case EnemyState.Attack:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;                        
                        attackCoolTime = attackInterval;
                        onStateUpdate = Update_Attack;
                        break; 
                    case EnemyState.Dead:
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Die");
                        onStateUpdate = Update_Dead;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 대기 상태로 들어갔을 때 기다리는 시간
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// 대기 시간 측정용(계속 감소)
    /// </summary>
    float waitTimer = 1.0f;

    /// <summary>
    /// 측정용 시간 처리용 프로퍼티
    /// </summary>
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0.0f) 
            {
                State = EnemyState.Patrol;
            }
        }
    }

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.0f;

    /// <summary>
    /// 적이 순찰할 웨이포인트(public이지만 private처럼 사용할 것)
    /// </summary>
    public Waypoints waypoints;

    /// <summary>
    /// 원거리 시야 범위
    /// </summary>
    public float farSightRange = 10.0f;

    /// <summary>
    /// 원거리 시야각의 절반
    /// </summary>
    public float sightHalfAngle = 50.0f;

    /// <summary>
    /// 근거리 시야 범위
    /// </summary>
    public float nearSightRange = 1.5f;

    /// <summary>
    /// 추적 대상의 트랜스폼
    /// </summary>
    protected Transform chaseTarget = null;

    /// <summary>
    /// 공격 대상
    /// </summary>
    protected IBattler attackTarget = null;

    /// <summary>
    /// 공격력(변수는 인스펙터에서 수정하기 위해 public으로 만든 것임)
    /// </summary>
    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    /// <summary>
    /// 방어력(변수는 인스펙터에서 수정하기 위해 public으로 만든 것임)
    /// </summary>
    public float defencePower = 3.0f;
    public float DefencePower => defencePower;

    /// <summary>
    /// 공격 속도
    /// </summary>
    public float attackInterval = 1.0f;

    /// <summary>
    /// 남아있는 공격 쿨타임
    /// </summary>
    float attackCoolTime = 0.0f;

    /// <summary>
    /// HP
    /// </summary>
    protected float hp = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if( State != EnemyState.Dead && hp <= 0)    // 한번만 죽기용도
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke(hp / MaxHP);
        }
    }

    /// <summary>
    /// 최대 HP(변수는 인스펙터에서 수정하기 위해 public으로 만든 것임)
    /// </summary>
    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// HP 변경시 실행되는 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 살았는지 죽었는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// 이 캐릭터가 죽었을 때 실행되는 델리게이트
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// 이 캐릭터가 맞았을 때 실행되는 델리게이트(int : 실제로 입은 데미지)
    /// </summary>
    public Action<int> onHit { get; set; }

    [System.Serializable]   // 이게 있어야 구조체 내용을 인스팩터 창에서 수정할 수 있다.
    public struct ItemDropInfo
    {
        public ItemCode code;       // 아이템 종류
        [Range(0,1)]
        public float dropRatio;     // 드랍 확율(1.0f = 100%)
        public uint dropCount;      // 최대 드랍 개수
    }
    /// <summary>
    /// 이 적이 죽을때 드랍하는 아이테 정보
    /// </summary>
    public ItemDropInfo[] dropItems;
    
    /// <summary>
    /// 상태별 업데이트 함수가 저장될 델리게이트(함수 저장용)
    /// </summary>
    Action onStateUpdate;

    /// <summary>
    /// 공격 이팩트가 생성될 위치
    /// </summary>
    Transform hitEffectPosition;

    // 컴포넌트들
    Animator animator;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rigid;
    EnemyHealthBar hpBar;
    ParticleSystem dieEffect;

    // 읽기 전용
    readonly Vector3 EffectResetPosition = new(0.0f, 0.01f, 0.0f);

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();

        Transform child = transform.GetChild(2);
        hpBar = child.GetComponent<EnemyHealthBar>();
        child = transform.GetChild(3);
        dieEffect = child.GetComponent<ParticleSystem>();

        child = transform.GetChild(4);
        AttackArea attackArea = child.GetComponent<AttackArea>();
        attackArea.onPlayerIn += (target) =>
        {
            // 플레이어가 들어온 상태에서
            if(State == EnemyState.Chase)   // 추적 상태이면
            {
                attackTarget = target;      // 공격 대상 지정하고
                State = EnemyState.Attack;  // 공격 상태로 변환
            }
        };
        attackArea.onPlayerOut += (target) =>
        {
            if( attackTarget == target )            // 공격 대상이 나갔으면
            {
                attackTarget = null;                // 공격 대상을 비우고
                if(State != EnemyState.Dead)        // 죽지 않았다면
                {
                    State = EnemyState.Chase;       // 추적 상태를 되돌리기
                }
            }
        };

        hitEffectPosition = transform.GetChild(5);  // 공격 이팩트가 나올 위치

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.speed = moveSpeed;            // 이동 속도 지정
        State = EnemyState.Wait;            // 기본 상태 지정
        animator.ResetTrigger("Stop");      // Wait 상태로 설정하면서 Stop 트리거가 쌓인 것을 제거하기 위해 필요
        rigid.isKinematic = true;           // 키네마틱을 꺼서 물리가 적용되게 만들기
        rigid.drag = Mathf.Infinity;        // 무한대로 되어 있던 마찰력을 낮춰서 떨어질 수 있게 하기
        HP = maxHP;                         // HP 최대로
    }

    protected override void OnDisable()
    {        
        bodyCollider.enabled = true;        // 컬라이더 활성화
        hpBar.gameObject.SetActive(true);   // HP바 다시 보이게 만들기
        agent.enabled = true;               // agent가 활성화 되어 있으면 항상 네브메시 위에 있음

        base.OnDisable();
    }

    void Update()
    {
        onStateUpdate();        
    }

    /// <summary>
    /// Wait 상태용 업데이트 함수
    /// </summary>
    void Update_Wait()
    {
        if( SearchPlayer() )
        {
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;    // 기다리는 시간 감소(0이되면 Patrol로 변경)

            // 다음 목적지를 바라보게 만들기
            Quaternion look = Quaternion.LookRotation(waypoints.NextTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 2);
        }
    }

    /// <summary>
    /// Patrol 상태용 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        if( SearchPlayer() )
        {
            State = EnemyState.Chase;
        }
        else
        {
            if( agent.remainingDistance <= agent.stoppingDistance ) // 도착하면
            {
                waypoints.StepNextWaypoint();   // 웨이포인트가 다음 지점을 설정하도록 실행
                State = EnemyState.Wait;        // 대기 상태로 전환
            }
        }
    }

    void Update_Chase()
    {
        if( SearchPlayer() )
        {
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            State = EnemyState.Wait;
        }
    }

    void Update_Attack()
    {
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if(attackCoolTime < 0)
        {            
            Attack(attackTarget);
        }
    }

    void Update_Dead()
    {
    }

    /// <summary>
    /// 시야 범위안에 플레이어가 있는지 없는지 찾는 함수
    /// </summary>
    /// <returns>찾았으면 true, 못찾았으면 false</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        // 일정 반경(=farSightRange)안에 있는 플레이어 레이어에 있는 오브젝트 전부 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));
        if( colliders.Length > 0 )
        {
            // 일정 반경(=farSightRange)안에 플레이어가 있다.
            Vector3 playerPos = colliders[0].transform.position;    // 0번이 무조건 플레이어다(플레이어는 1명이니까)
            Vector3 toPlayerDir = playerPos - transform.position;   // 적->플레이어로 가는 방향 백터
            if(toPlayerDir.sqrMagnitude < nearSightRange * nearSightRange)  // 플레이어는 nearSightRange보다 안쪽에 있다.
            {
                // 근접범위(=nearSightRange) 안쪽이다.
                chaseTarget = colliders[0].transform;
                result = true;
            }
            else
            {
                // 근접범위 밖이다 => 시야각 확인
                if(IsInSightAngle(toPlayerDir))     // 시야각 안인지 확인
                { 
                    if(IsSightClear(toPlayerDir))   // 적과 플레이어 사이에 시야를 가리는 오브젝트가 있는지 확인
                    {
                        chaseTarget = colliders[0].transform;
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 시야각(-sightHalfAngle ~ +sightHalfAngle)안에 플레이어가 있는지 없는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">적에서 대상으로 향하는 방향 백터</param>
    /// <returns>시야각 안에 있으면 true, 없으면 false</returns>
    bool IsInSightAngle(Vector3 toTargetDirection)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDirection);  // 적의 포워드와 적을 바라보는 방향백터 사이의 각을 구함
        return sightHalfAngle > angle;
    }

    /// <summary>
    /// 적이 다른 오브젝트에 의해 가려지는지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">적에서 대상으로 향하는 방향 백터</param>
    /// <returns>true면 가려지지 않는다. false면 가려진다.</returns>
    bool IsSightClear(Vector3 toTargetDirection)
    {
        bool result = false;
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDirection); // 래이 생성(눈 높이 때문에 조금 높임)
        if(Physics.Raycast(ray, out RaycastHit hitInfo, farSightRange)) 
        { 
            if(hitInfo.collider.CompareTag("Player"))   // 처음 충돌한 것이 플레이어라면
            {
                result = true;                          // 중간에 가리는 물체가 없다는 소리
            }
        }

        return result;
    }

    /// <summary>
    /// 공격처리용 함수
    /// </summary>
    /// <param name="target">공격 대상</param>
    public void Attack(IBattler target)
    {
        animator.SetTrigger("Attack");      // 애니메이션 재생
        target.Defence(AttackPower);        // 공격 대상에게 데미지 전달

        Factory.Instance.GetEnemyHitEffect(hitEffectPosition.position + UnityEngine.Random.insideUnitSphere * 0.1f);

        attackCoolTime = attackInterval;    // 쿨타임 초기화
    }

    /// <summary>
    /// 방어 처리용 함수
    /// </summary>
    /// <param name="damage">내가 받은 순수 데미지</param>
    public void Defence(float damage)
    {
        if(IsAlive) // 살아있을 때만 데미지를 받음
        {
            animator.SetTrigger("Hit");                 // 애니메이션 재생

            float final = Mathf.Max(0, damage - DefencePower);  // 최종 데미지 계산해서 적용
            HP -= final;
            onHit?.Invoke(Mathf.RoundToInt(final));
            //Debug.Log($"적이 맞았다. 남은 HP = {HP}");
        }
    }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    public void Die()
    {
        //Debug.Log("사망");
        State = EnemyState.Dead;        // 상태 변경
        StartCoroutine(DeadSquence());  // 사망 연출 시작
        onDie?.Invoke();                // 죽었다고 알림 보내기
    }

    /// <summary>
    /// 사망 연출용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadSquence()
    {
        // 컬라이더 비활성화
        bodyCollider.enabled = false;

        // 사망 이팩트 처리
        dieEffect.Play();                       // 재생 시작
        dieEffect.transform.SetParent(null);    // 이팩트를 부모에서 분리(같이 가라앉는 것 방지)

        // HP바 안보이게 만들기
        hpBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);  // 아이템이 바로 떨어지면 어색해서 약간 대기

        // 아이템 드랍
        MakeDropItems();

        // 사망 애니메이션 끝날때까지 대기
        yield return new WaitForSeconds(1.0f);  // 사망 애니메이션 시간(1.333초) -> 1.5초로 처리

        // 바닥으로 가라 앉기 시작
        agent.enabled = false;                  // agent가 활성화 되어 있으면 항상 네브메시 위에 있음
        rigid.isKinematic = false;              // 키네마틱을 꺼서 물리가 적용되게 만들기
        rigid.drag = 10.0f;                     // 무한대로 되어 있던 마찰력을 낮춰서 떨어질 수 있게 하기

        // 충분히 바닥아래로 내려갈때까지 대기
        yield return new WaitForSeconds(2.0f);  // 2초면 다 떨어질 것이다.

        // 슬라임 풀로 되돌리기
        dieEffect.transform.SetParent(this.transform);              // 이팩트 부모 되돌리고
        dieEffect.transform.localPosition = EffectResetPosition;    // 위치 리셋
        gameObject.SetActive(false);    // 즉시 슬라임 풀로 되돌리기
    }

    /// <summary>
    /// 아이템을 드랍하는 함수
    /// </summary>
    void MakeDropItems()
    {
        // dropItems; 이 정보를 바탕으로 아이템을 드랍
        foreach(var item in dropItems)
        {
            if( item.dropRatio > UnityEngine.Random.value ) // 확률 체크하고
            {
                uint count = (uint)UnityEngine.Random.Range(0, item.dropCount) + 1;     // 개수 결정
                Factory.Instance.MakeItems(item.code, count, transform.position, true); // 실제 생성
            }
        }
    }

    public void HealthRegenerate(float totalRegen, float duration)
    {
        throw new NotImplementedException();
    }

    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {
        throw new NotImplementedException();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        bool playerShow = SearchPlayer();
        Handles.color = playerShow ? Color.red : Color.green;

        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f); // 중심선 그리기

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);            // 중심선 회전시키고
        Handles.DrawLine(transform.position, transform.position + q1 * forward);        // 선 긋기

        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, farSightRange, 2.0f);   // 호 그리기

        Handles.DrawWireDisc(transform.position, transform.up, nearSightRange);         // 근거리 시야 범위 그리기
    }

    public void Test_DropItems(int testCount)
    {
        uint[] types = new uint[dropItems.Length];
        uint[] total = new uint[dropItems.Length];

        for(int i=0;i<testCount;i++)
        {
            int index = 0;
            foreach (var item in dropItems)
            {
                if (item.dropRatio > UnityEngine.Random.value)
                {
                    uint count = (uint)UnityEngine.Random.Range(0, item.dropCount) + 1;
                    //Factory.Instance.MakeItems(item.code, count, transform.position, true);
                    types[index]++;
                    total[index] += count;
                }
                index++;
            }
        }

        Debug.Log($"1st : {types[0]}번 드랍, {total[0]}개 드랍");
        Debug.Log($"2nd : {types[1]}번 드랍, {total[1]}개 드랍");
    }
#endif
}
