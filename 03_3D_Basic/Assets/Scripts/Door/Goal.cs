using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    /// <summary>
    /// 골인했을 때 불꽃놀이 이팩트들이 들어갈 배열
    /// </summary>
    ParticleSystem[] fireworks;

    private void Awake()
    {
        // 터트릴 불꽃놀이 이팩트 모두 찾아놓기
        Transform fireworkTransform = transform.GetChild(2);
        fireworks = new ParticleSystem[fireworkTransform.childCount];

        for(int i = 0; i < fireworks.Length; i++)
        {
            Transform child = fireworkTransform.GetChild(i);
            fireworks[i] = child.GetComponent<ParticleSystem>();
        }
    }

    private void Start()
    {
        GameManager.Instance.onGameClear += OnGoalIn;   // 게임 클리어했을 때 OnGoalIn함수 실행
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") )
        {
            OnGoalIn();
            StartCoroutine(GameClear());
        }
    }

    /// <summary>
    /// 플레이어가 골인 했을 때 실행될 함수
    /// </summary>
    private void OnGoalIn()
    {
        foreach (var firework in fireworks)
        {
            firework.Play();    // 모든 폭죽 터트리기
        }

        StartCoroutine(FireworkEffect());
    }

    IEnumerator FireworkEffect()
    {
        while (true)
        {
            // fireworks 셔플하기(피셔 예이츠 알고리즘)
            for (int i = fireworks.Length - 1; i > -1; i--)
            {
                int index = Random.Range(0, i);
                        
                (fireworks[index], fireworks[i]) = (fireworks[i], fireworks[index]);    // 두 값을 스왑하기
            }

            // 순차적으로 터트리기
            for(int i=0;i < fireworks.Length;i++)
            {
                yield return new WaitForSeconds(0.5f);
                fireworks[i].Play();
            }
        }
    }

    IEnumerator GameClear()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.GameClear();   // 플레이어가 트리거에 들어오면 게임 클리어
    }
}