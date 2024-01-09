using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI score;

    /// <summary>
    /// 목표로 하는 최종 점수
    /// </summary>
    int goalScore = 0;

    /// <summary>
    /// 현재 보여지는 점수
    /// </summary>
    float currentScore = 0.0f;

    /// <summary>
    /// 점수가 올라가는 속도
    /// </summary>
    public float scoreUpSpeed = 50.0f;

    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        player.onScoreChange += RefreshScore;

        goalScore = 0;
        currentScore = 0.0f;
        score.text = "Score : 00000";
    }

    private void Update()
    {
        if(currentScore < goalScore)    // 점수가 올라가는 도중일 때
        {
            float speed = Mathf.Max((goalScore - currentScore) * 5.0f, scoreUpSpeed);   // 최소 scoreUpSpeed 보장

            currentScore += Time.deltaTime * speed;
            currentScore = Mathf.Min(currentScore, goalScore);

            int temp = (int)currentScore;
            score.text = $"Score : {temp:d5}";
            //score.text = $"Score : {currentScore:f0}";    // 소수점 출력 안하기
        }
    }

    private void RefreshScore(int newScore)
    {
        //score.text = $"Score : {newScore:d5}";  // 무조건 점수는 5자리로 출력. 빈자리는 0으로 채우기
        //score.text = $"Score : {newScore,5}"; // 무조건 점수는 5자리로 출력. 빈자리는 스페이스로 채우기

        goalScore = newScore;
    }
}
