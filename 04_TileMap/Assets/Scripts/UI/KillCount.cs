using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCount : MonoBehaviour
{
    public float countingSpeed = 0.5f;

    float target = 0.0f;
    float current = 0.0f;

    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onKillCountChange += OnKillCountChange;
    }

    private void Update()
    {
        current += Time.deltaTime * countingSpeed;  // current는 target까지 지속적으로 증가
        if(current > target)
        {
            current = target;   // 넘치는 것 방지
        }
        imageNumber.Number = Mathf.FloorToInt(current);
    }

    private void OnKillCountChange(int count)
    {
        //imageNumber.Number = count;
        target = count;     // 새 킬카운트를 target으로 지정
    }
}
