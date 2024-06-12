using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDirection : MonoBehaviour
{
    public float duration = 0.5f;
    float timeElapsed = 0.0f;
    float inverseDuration;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        inverseDuration = 1 / duration;

        GameManager.Instance.Player.onAttacked += OnPlayerAttacked;
        timeElapsed = duration;
        image.color = Color.clear;
    }

    private void OnPlayerAttacked(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        image.color = Color.white;
        timeElapsed = 0;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float alpha = timeElapsed * inverseDuration;
        image.color = Color.Lerp(Color.white, Color.clear, alpha);  // 항상 흰색에서 투명색으로 보간이 일어남
    }
}
