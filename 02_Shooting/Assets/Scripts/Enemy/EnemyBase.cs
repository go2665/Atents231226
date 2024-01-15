using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 데이터")]
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 적의 HP
    /// </summary>
    int hp = 1;

    private int HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0) // HP가 0 이하가 되면 죽는다.
            {
                hp = 0;
                OnDie();
            }
        }
    }

    /// <summary>
    /// 최대 HP
    /// </summary>
    public int maxHP = 1;

    /// <summary>
    /// 이 적을 해치웠을 때 플레이어가 얻는 점수
    /// </summary>
    public int score = 10;

    /// <summary>
    /// 적이 죽을 때 실행될 델리게이트
    /// </summary>
    Action onDie;

    /// <summary>
    /// 점수를 줄 플레이어
    /// </summary>
    Player player;


    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();     // 적 초기화 작업
    }

    protected override void OnDisable()
    {
        if(player != null)
        {
            onDie -= PlayerAddScore;    // 순차적으로 제거
            onDie = null;               // 확실하게 정리한다고 표시
            player = null;
        }

        base.OnDisable();
    }

    private void Update()
    {
        OnMoveUpdate(Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet")
            || collision.gameObject.CompareTag("Player"))   // 총알이 부딪치거나 플레이어와 부딪치면 HP가 1 감소한다.
        {
            HP--;
        }
    }

    /// <summary>
    /// EnemyWave 계열의 초기화 함수
    /// </summary>
    protected virtual void OnInitialize()
    {
        if( player == null )
        {
            player = GameManager.Instance.Player;   // 플레이어 찾기
        }

        if( player != null )
        {
            onDie += PlayerAddScore;                // 플레이어 점수 증가 함수 등록
        }

        HP = maxHP; // HP 최대로 설정
    }

    /// <summary>
    /// 업데이트 중에 이동 처리하는 함수
    /// </summary>
    /// <param name="deltaTime">프레임간의 간격</param>
    protected virtual void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * moveSpeed * -transform.right, Space.World); // 기본동작(왼쪽으로 이동)
    }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    protected virtual void OnDie()
    {
        // 터지는 이팩트 생성
        Factory.Instance.GetExplosionEffect(transform.position);

        onDie?.Invoke();                // 죽었다는 신호보내기

        gameObject.SetActive(false);    // 자기 자신 비활성화
    }

    /// <summary>
    /// 플레이어 점수 추가용 함수(델리게이트 등록용)
    /// </summary>
    void PlayerAddScore()
    {
        player.AddScore(score);
    }
}

