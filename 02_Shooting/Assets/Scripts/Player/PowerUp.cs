using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : RecycleObject
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 방향이 전환되는 시간 간격
    /// </summary>
    public float dirChangeInterval = 1.0f;

    /// <summary>
    /// 방향전환이 가능한 회수(최대치)
    /// </summary>
    public int dirChangeCountMax = 5;

    /// <summary>
    /// 남아있는 방향 전환 회수
    /// </summary>
    int dirChangeCount = 5;

    /// <summary>
    /// 방향 전환 회수 설정 및 확인용 프로퍼티
    /// </summary>
    int DirChangeCount
    {
        get => dirChangeCount;
        set
        {
            dirChangeCount = value;                         // 값을 변경시키고
            animator.SetInteger("Count", dirChangeCount);   // 애니메이터의 파라메터 수정

            StopAllCoroutines();        // 이전에 돌아가던 코루틴 정지(벽에 부딪쳤을 떄 필요)

            if(dirChangeCount > 0 && gameObject.activeSelf) // 방향전환 회수가 남아있고 활성화 중이면
            {
                //Debug.Log($"방향전환 남은 회수 : {DirChangeCount}");
                StartCoroutine(DirectionChange());          // 일정 시간 후에 방향을 전환하는 코루틴 실행
            }
        }
    }

    /// <summary>
    /// 현재 이동 방향
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// 플레이어의 트랜스폼
    /// </summary>
    Transform playerTransform;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StopAllCoroutines();                // 혹시나 실행되고 있을지도 모르는 모든 코루틴 정지

        playerTransform = GameManager.Instance.Player.transform;
        direction = Vector3.zero;           // 방향 0로 해서 안움직이게 
        DirChangeCount = dirChangeCountMax; // 방향전환 회수 초기화
    }

    /// <summary>
    /// 주기적으로 방향을 전환하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DirectionChange()
    {
        yield return new WaitForSeconds(dirChangeInterval);
            
        // 약 70% 확률로 플레이어 반대방향으로 움직임
        if(Random.value < 0.4f)
        {
            // 플레이어 반대방향
            Vector2 playerToPowerUp = transform.position - playerTransform.position;    // 방향 백터 구하고
            direction = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerUp;  // +-90도 사이로 회전    
        }
        else
        {
            direction = Random.insideUnitCircle;    // 반지름 1짜리 원 내부의 랜덤한지점으로 가는 방향 저장
            // 모든 방향이니 50%확률로 플레이어 반대방향
        }            
            
        direction.Normalize();                  // 구한 방향의 크기를 1로 설정
                                                //direction = Vector3.up; // 테스트코드
        
        DirChangeCount--;                       // 방향전환 회수 감소
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * direction);    // 항상 direction 방향으로 이동
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(DirChangeCount > 0 && collision.gameObject.CompareTag("Border"))   // 보더랑 부딪치면
        {
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);   // 이동 방향 반사시키기
            DirChangeCount--;
        }
    }
}