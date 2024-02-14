using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI text;

    /// <summary>
    /// 원래 값을 구하기 위해 사용될 최대 값
    /// </summary>
    float maxValue = 0.0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += Refresh;
        player.onDie += Stop;
        maxValue = player.startLifeTime;
    }

    private void Stop()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange -= Refresh;
        player.onDie -= Stop;
    }

    private void Refresh(float ratio)
    {
        slider.value = ratio;
        text.text = $"{(ratio * maxValue):f1} Sec"; // 비율에 최대 값을 곱해서 원래 값으로 변경
    }
}
