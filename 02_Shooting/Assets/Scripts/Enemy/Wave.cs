using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : EnemyBase
{
    // 실습
    // 1. 적은 위아래로 파도치듯이 움직인다.
    // 2. 적은 계속 왼쪽 방향으로 이동한다.

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
    

    protected override void OnEnable()
    {
        base.OnEnable();

        // 초기화
        spawnY = transform.position.y;
        elapsedTime = 0.0f;

        //Action aaa = () => Debug.Log("람다함수");             // 파라메터 없는 람다식
        //Action<int> bbb = (x) => Debug.Log($"람다함수 {x}");  // 파라메터가 하나인 람다식
        //Func<int> ccc = () => 10;                            // 파라메터 없고 항상 10을 리턴하는 람다식        

    }

    /// <summary>
    /// 시작 위치 설정을 위한 함수
    /// </summary>
    /// <param name="position">시작 위치</param>
    public void SetStartPosition(Vector3 position)
    {
        transform.position = position;
        spawnY = position.y;
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        //elapsedTime += Time.deltaTime;  // 시작부터 진행된 시간 측정
        elapsedTime += deltaTime * frequency;  // sin 그래프의 진행을 더 빠르게 만들기

        transform.position = new Vector3(transform.position.x - deltaTime * speed, // 계속 왼쪽으로 진행
            spawnY + Mathf.Sin(elapsedTime) * amplitude,    // sin 그래프에 따라 높에 변동하기
            0.0f);
    }
}

// 실습
// 1. 적에게 HP 추가(3대를 맞으면 폭발)
// 2. 적이 폭발할 때 explosionEffect 생성
// 3. 플레이어 총알 발사할때 flash 잠깐 보이기
// 4. 연사처리
