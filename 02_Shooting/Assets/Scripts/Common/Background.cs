using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float scrollingSpeed = 2.5f;

    /// <summary>
    /// 그림 가로 길이
    /// </summary>
    const float BackgroundWidth = 13.6f;

    /// <summary>
    /// 움직일 대상
    /// </summary>
    Transform[] bgSlots;

    /// <summary>
    /// 오른쪽 끝으로 보내는 기준
    /// </summary>
    float baseLineX;

    protected virtual void Awake()
    {
        bgSlots = new Transform[transform.childCount];  // 배열 만들고
        for(int i = 0; i < bgSlots.Length; i++)
        {
            bgSlots[i] = transform.GetChild(i);         // 배열에 자식을 하나씩 넣기
        }

        baseLineX = transform.position.x - BackgroundWidth; // 기준이될 x위치 구하기
    }

    private void Update()
    {
        for(int i = 0;i < bgSlots.Length;i++)
        {
            bgSlots[i].Translate(Time.deltaTime * scrollingSpeed * -transform.right);   // 이동 대상을 계속 왼쪽으로 이동 시키기

            if (bgSlots[i].position.x < baseLineX)  // 기준선을 넘었는지 확인하고
            {
                MoveRight(i);                       // 넘었으면 오른쪽 끝으로 보내기
            }
        }
    }

    /// <summary>
    /// 오른쪽 끝으로 이동시키는 함수
    /// </summary>
    /// <param name="index">이동시킬 대상의 인덱스</param>
    protected virtual void MoveRight(int index)
    {
        bgSlots[index].Translate(BackgroundWidth * bgSlots.Length * transform.right);   // 들어있는 개수  * 가로길이 만큼 오른쪽으로 보내기
    }
}
