using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages;

    Image[] digits;

    /// <summary>
    /// 목표값
    /// </summary>
    int number = 0;

    public int Number
    {
        get => number;
        set
        {
            if(number != value)
            {
                number = value;
            }
        }
    }

    private void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }
}

// 실습
// number는 5째자리까지 표현 가능(max = 99999)
// number에 값을 세팅하면 digits에 적절한 이미지가 선택된다.
// 사용되지 않는 자리는 disable처리