using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    TextMeshProUGUI score;

    private void Awake()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        player.onScoreChange += RefreshScore;
    }

    private void RefreshScore(int newScore)
    {
        score.text = $"Score : {newScore:d5}";  // 무조건 점수는 5자리로 출력. 빈자리는 0으로 채우기
        //score.text = $"Score : {newScore,5}"; // 무조건 점수는 5자리로 출력. 빈자리는 스페이스로 채우기
    }
}
