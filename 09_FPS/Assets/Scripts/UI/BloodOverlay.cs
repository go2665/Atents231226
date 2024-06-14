using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOverlay : MonoBehaviour
{
    public AnimationCurve curve;
    public Color color = Color.clear;

    Image image;

    float inverseMaxHP;

    float targetAlpha = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = color;
    }

    private void Start()
    {
        GameManager.Instance.Player.onHPChange += OnHPChange;
        inverseMaxHP = 1 / GameManager.Instance.Player.MaxHP;   // 비율 계산할 때 /대신 *로 처리하기 위해 미리 계산해 놓기
    }

    private void Update()
    {
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime);
        image.color = color;
    }

    private void OnHPChange(float health)
    {
        targetAlpha = curve.Evaluate(1 - (health * inverseMaxHP));        
    }
}
