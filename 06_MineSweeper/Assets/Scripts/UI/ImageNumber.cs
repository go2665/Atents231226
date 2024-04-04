using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageNumber : MonoBehaviour
{
    // 숫자를 입력받아 Image로 표현하는 클래스

    int number = 0;
    public int Number
    {
        get => number;
        set
        {
            // 값의 범위는 -99~999
        }
    }
}
