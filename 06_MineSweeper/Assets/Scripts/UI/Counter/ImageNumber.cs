using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    // 숫자를 입력받아 Image로 표현하는 클래스

    /// <summary>
    /// 숫자 스프라이트의 배열
    /// </summary>
    public Sprite[] numberSprites;

    // 가독성을 위해 일부 스프라이트를 프로퍼티로 설정
    Sprite ZeroSprite => numberSprites[0];      // 0
    Sprite MinusSprite => numberSprites[11];    // -
    Sprite EmptySprite => numberSprites[10];    // 빈칸

    /// <summary>
    /// 자리수별 이미지 컴포넌트(0:1자리, 1:10자리, 2:100자리)
    /// </summary>
    Image[] numberDigits;

    int number = -1;
    public int Number
    {
        get => number;
        set
        {
            if(number != value) // 숫자가 변했을 때만 실행
            {
                number = Mathf.Clamp(value, -99, 999);  // 값의 범위는 -99~999
                Refresh();
            }
        }
    }

    private void Awake()
    {
        numberDigits = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// Number가 변경되었을 때 실행될 함수
    /// </summary>
    void Refresh()
    {
        int temp = Mathf.Abs(Number);           // 일단 양수로 처리

        Queue<int> digits = new Queue<int>(3);  // temp 자리수별로 자른 수를 저장할 큐

        // 자리수별로 Number를 나누어서 digits에 담기
        while(temp > 0)
        {
            digits.Enqueue(temp % 10);  // %연산으로 마지막 자리 잘라내기
            temp /= 10;                 // 잘라낸 부분 제거
        }

        // digits에 저장된 데이터를 기반으로 이미지 표시하기
        int index = 0;
        while(digits.Count > 0)
        {
            int num = digits.Dequeue();                         // 큐에서 하나씩 꺼낸 후
            numberDigits[index].sprite = numberSprites[num];    // 스프라이트 설정
            index++;
        }

        // 남은 칸에 0으로 이미지 설정하기
        for(int i = index;i<numberDigits.Length;i++)
        {
            numberDigits[i].sprite = ZeroSprite;    // 빈칸은 무조건 0
        }

        // 원래 음수였을 경우의 처리
        if(Number < 0)
        {
            numberDigits[numberDigits.Length - 1].sprite = MinusSprite; // 앞에 -붙이기
        }
    }
}
