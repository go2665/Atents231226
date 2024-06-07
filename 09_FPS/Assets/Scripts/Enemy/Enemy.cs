using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적이 맞을 수 있는 부위
/// </summary>
public enum HitLocation : byte
{
    Body,
    Head,
    Arm,
    Leg
}

public class Enemy : MonoBehaviour
{
    // HP관련 ------------------------------------------------------------------------------------------
    
    /// <summary>
    /// 현재 HP
    /// </summary>
    [SerializeField]    // 테스트용
    float hp = 30.0f;
    
    /// <summary>
    /// 최대 HP
    /// </summary>
    [SerializeField]    // 성능을 추구한다면 비권장. 객체지향적으로는 더 올바름
    float maxHP = 30.0f;

    /// <summary>
    /// HP 설정 및 확인용 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                State = BehaviorState.Dead; // HP가 0이하면 사망
            }
        }
    }

    /// <summary>
    /// 사망시 실행될 델리게이트
    /// </summary>
    public Action<Enemy> onDie;

    // 상태 관련 ------------------------------------------------------------------------------------------
    public enum BehaviorState : byte
    {
        Wander = 0, // 배회상태. 주변을 왔다갔다한다.
        Chase,      // 추적상태. 플레이어가 마지막으로 목격된 장소를 향해 계속 이동한다.
        Find,       // 탐색상태. 추적 도중에 플레이어가 시야에서 사라지면 두리번 거리며 주변을 찾는다.
        Attack,     // 공격상태. 추적 상태일 때 플레이어가 일정범위안에 들어오면 일정 주기로 공격을 한다.
        Dead        // 사망상태. 죽는다.(일정 시간 후에 재생성)
    }

    /// <summary>
    /// 적의 현재 상태
    /// </summary>
    BehaviorState state = BehaviorState.Dead;

    /// <summary>
    /// 적의 상태 확인 및 설정용 프로퍼티
    /// </summary>
    BehaviorState State
    {
        get => state;
        set
        {
            if(state != value)          // 상태가 달라지면
            {
                OnStateExit(value);     // 이전 상태에서 나가기 처리 실행
                state = value;
                OnStateEnter(value);    // 새 상태에 들어가기 처리 실행
            }
        }
    }

    /// <summary>
    /// 각 상태가 되었을때 상태별 업데이트 함수를 저장하는 델리게이트(함수포인터의 역할)
    /// </summary>
    Action onUpdate = null;

    // 이동 관련 ----------------------------------------------------------------------------------------
    
    /// <summary>
    /// 이동 속도(배회 및 찾기 상태에서 사용)
    /// </summary>
    public float walkSpeed = 2.0f;

    /// <summary>
    /// 이동 속도(추적 및 공격 상태에서 사용)
    /// </summary>
    public float runSpeed = 7.0f;

    /// <summary>
    /// 이동 패널티(다리를 맞으면 증가)
    /// </summary>
    float speedPenalty = 0;

    // 시야 관련 --------------------------------------------------------------------------------------------

    /// <summary>
    /// 적의 시야각
    /// </summary>
    public float sightAngle = 90.0f;

    /// <summary>
    /// 적의 시야 범위
    /// </summary>
    public float sightRange = 20.0f;

    // 공격 관련 --------------------------------------------------------------------------------------------
    
    /// <summary>
    /// 공격 대상
    /// </summary>
    Player attackTarget = null;

    /// <summary>
    /// 공격력
    /// </summary>
    public float attackPower = 10.0f;

    /// <summary>
    /// 공격 시간 간격
    /// </summary>
    public float attackInterval = 1.0f;

    /// <summary>
    /// 공격 시간 측정용
    /// </summary>
    float attackElapsed = 0;

    /// <summary>
    /// 공격력 패널티 정도
    /// </summary>
    float attackPowerPenalty = 0;

    // 탐색 관련 -------------------------------------------------------------------------------------------
    
    /// <summary>
    /// 탐색 상태에서 배회 상태로 돌아가기까지 걸리는 시간
    /// </summary>
    public float findTime = 5.0f;

    /// <summary>
    /// 탐색 진행 시간
    /// </summary>
    float findTimeElapsed = 0.0f;

    // 기타 -------------------------------------------------------------------------------------------------
        
    NavMeshAgent agent;


    // --------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void Update()
    {
        onUpdate();
    }

    void Update_Wander()
    {

    }

    void Update_Chase()
    {

    }

    void Update_Find()
    {

    }

    void Update_Attack()
    {

    }

    void Update_Dead()
    {

    }

    /// <summary>
    /// 특정 상태가 되었을 때의 처리를 실행하는 함수
    /// </summary>
    /// <param name="newState">새 상태</param>
    void OnStateEnter(BehaviorState newState)
    {

    }

    /// <summary>
    /// 특정 샅애에서 나갈때의 처리를 실행하는 함수
    /// </summary>
    /// <param name="oldState">옛 상태</param>
    void OnStateExit(BehaviorState oldState)
    {

    }

    /// <summary>
    /// 배회하기 위해 랜덤한 위치를 돌려주는 함수
    /// </summary>
    /// <returns>랜덤한 배회용 목적지</returns>
    Vector3 GetRandomDestination()
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 플레이어를 공격하는 함수
    /// </summary>
    void Attack()
    {

    }

    /// <summary>
    /// 공격 당함을 처리하는 함수
    /// </summary>
    /// <param name="hit">맞은 부위</param>
    /// <param name="damage">데미지</param>
    public void OnAttacked(HitLocation hit, float damage)
    {
        // 맞으면 즉시 추적에 돌입한다.
    }

    /// <summary>
    /// 플레이어를 찾는 시도를 함수
    /// </summary>
    /// <returns>true면 플레이어를 찾았다. false면 못찾았다.</returns>
    bool FindPlayer()
    {
        return false;
    }

    /// <summary>
    /// 플레이어가 시야범위 안에 있는지 확인하는 함수
    /// </summary>
    /// <param name="position">플레이어가 시야범위 안에 있을 때 플레이어의 위치</param>
    /// <returns>true면 시야범위 안에 있다. false면 시야범위 안에 없다.</returns>
    bool IsPlayerInSight(out Vector3 position)
    {
        position = Vector3.zero;
        return false;
    }

    /// <summary>
    /// 주변을 두리번 거리는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LookAround()
    {
        yield return null;
    }


    /// <summary>
    /// 적을 리스폰 시키는 함수
    /// </summary>
    /// <param name="spawnPosition">리스폰할 위치</param>
    public void Respawn(Vector3 spawnPosition)
    {
        State = BehaviorState.Wander;
    }

    /// <summary>
    /// 적이 드랍할 아이템의 종류를 나타내는 enum
    /// </summary>
    enum ItemTable : byte
    {
        Heal,           // 힐 아이템
        AssaultRifle,   // 돌격소총
        Shotgun,        // 샷건
        Random          // 랜덤
    }

    /// <summary>
    /// 아이템을 드랍하는 함수
    /// </summary>
    /// <param name="table">드랍할 아이템</param>
    void DropItem(ItemTable table = ItemTable.Random)
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 시야 각 그리기(상태별로 시야각 다른 색으로 그리기)
        
    }
#endif
}
