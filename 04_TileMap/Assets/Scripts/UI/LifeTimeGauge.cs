using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeTimeGauge : MonoBehaviour
{
    // 플레이어 수명에 따라 슬라이더의 value가 1 -> 0으로 움직인다.
    Slider slider;
    Image fill;

    //public Color startColor = Color.white;
    //public Color endColor = Color.red;
    public Gradient color;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;

        Transform child = transform.GetChild(1);
        child = child.GetChild(0);
        fill = child.GetComponent<Image>();
    }

    private void Start()
    {
        GameManager.Instance.Player.onLifeTimeChange += OnLifeTimeChange;
    }

    private void OnLifeTimeChange(float ratio)
    {
        slider.value = ratio;

        //fill.color = Color.Lerp(endColor, startColor, ratio);
        fill.color = color.Evaluate(ratio);
    }
}
