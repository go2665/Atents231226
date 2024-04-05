using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트가 들어간 게임 오브젝트는 반드시 ImageNumber가 있다.(없으면 자동으로 넣는다)
[RequireComponent(typeof(ImageNumber))] 
public class CounterBase : MonoBehaviour
{
    ImageNumber imageNumber;

    protected virtual void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    /// <summary>
    /// imageNumber를 갱신해서 표시되는 내용 변경하는 함수
    /// </summary>
    /// <param name="count">imageNumber가 표시할 수</param>
    protected void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
