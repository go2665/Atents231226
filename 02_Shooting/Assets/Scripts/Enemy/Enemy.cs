using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : RecycleObject
{
    // 실습
    // 1. 적은 위아래로 파도치듯이 움직인다.
    // 2. 적은 계속 왼쪽 방향으로 이동한다.

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// 위 아래로 움직이는 정도
    /// </summary>
    public float amplitude = 3.0f;

    /// <summary>
    /// 사인 그래프가 한번 왕복하는데 걸리는 시간 증폭용
    /// </summary>
    public float frequency = 2.0f;

    /// <summary>
    /// 적이 스폰된 높이
    /// </summary>
    float spawnY = 0.0f;

    /// <summary>
    /// 전체 경과 시간(frequency에 의해 증폭됨)
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 적의 HP
    /// </summary>
    public int hp = 3;

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
    /// 터질때 나올 이팩트
    /// </summary>
    public GameObject explosionPrefab;

    /// <summary>
    /// 이 적을 해치웠을 때 플레이어가 얻는 점수
    /// </summary>
    public int score = 10;

    /// <summary>
    /// 적이 죽을 때 실행될 델리게이트
    /// </summary>
    Action onDie;

    // 람다식, 람다함수(Lambda)
    // 익명 함수
    // 한 줄짜리 임시 함수, 1회용

    Player player;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();     // 적 초기화 작업

        // 초기화
        spawnY = transform.position.y;
        elapsedTime = 0.0f;

        //Action aaa = () => Debug.Log("람다함수");             // 파라메터 없는 람다식
        //Action<int> bbb = (x) => Debug.Log($"람다함수 {x}");  // 파라메터가 하나인 람다식
        //Func<int> ccc = () => 10;                            // 파라메터 없고 항상 10을 리턴하는 람다식        

    }

    protected override void OnDisable()
    {
        if(player != null)
        {
            onDie -= PlayerAddScore;
            onDie = null;
            player = null;
        }

        base.OnDisable();

    }

    void PlayerAddScore()
    {
        player.AddScore(score);
    }

    private void Update()
    {
        //elapsedTime += Time.deltaTime;  // 시작부터 진행된 시간 측정
        elapsedTime += Time.deltaTime * frequency;  // sin 그래프의 진행을 더 빠르게 만들기

        transform.position = new Vector3(transform.position.x - Time.deltaTime * speed, // 계속 왼쪽으로 진행
            spawnY + Mathf.Sin(elapsedTime) * amplitude,    // sin 그래프에 따라 높에 변동하기
            0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))   // 총알이 부딪치면 HP가 1 감소한다.
        {
            HP--;
        }
    }

    private void OnInitialize()
    {
        if( player == null )
        {
            player = GameManager.Instance.Player;
        }

        if( player != null )
        {
            //onDie += () => player.AddScore(score);          // 죽을 때 플레이어의 AddScore함수에 파라메터로 score넣고 실행하도록 등록
            onDie += PlayerAddScore;
        }
    }

    private void OnDie()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //Player player = FindAnyObjectByType<Player>();
        //player.AddScore(score);

        onDie?.Invoke();

        Destroy(gameObject);    // 자기 자신 삭제
    }

}

// 실습
// 1. 적에게 HP 추가(3대를 맞으면 폭발)
// 2. 적이 폭발할 때 explosionEffect 생성
// 3. 플레이어 총알 발사할때 flash 잠깐 보이기
// 4. 연사처리
