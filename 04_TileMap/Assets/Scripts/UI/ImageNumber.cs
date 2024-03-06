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
    int number = -1;

    /// <summary>
    /// 숫자를 확인하고 설정하는 프로퍼티
    /// </summary>
    public int Number
    {
        get => number;
        set
        {
            if(number != value)
            {
                number = Mathf.Min(value, 99999);   // 최대 5자리로 숫자 설정

                int temp = number;                  // 임시 변수에 number복사
                for(int i = 0;i<digits.Length;i++)
                {
                    if(temp != 0 || i == 0)                     // temp가 0이 아니면 처리
                    {
                        int digit = temp % 10;                  // 1자리 숫자 추출하기
                        digits[i].sprite = numberImages[digit]; // 추출한 숫자에 맞게 이미지 선택
                        digits[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        digits[i].gameObject.SetActive(false);  // temp가 0이면 그 자리수는 안보이게 만들기(1자리 제외)
                    }
                    temp /= 10;                                 // 1자리 수 제거하기
                }
            }
        }
    }

    private void Awake()
    {
        digits = GetComponentsInChildren<Image>();
    }
}