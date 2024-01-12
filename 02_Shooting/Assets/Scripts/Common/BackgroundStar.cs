using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStar : Background
{
    // 실습
    // MoveRight가 실행될 때마다 SpriteRenderer의 flip값이 랜덤으로 지정된다.
    SpriteRenderer[] spriteRenderers;

    protected override void Awake()
    {
        base.Awake();

        //SpriteRenderer sp = GetComponent<SpriteRenderer>();
        //sp.flipX;
        //sp.flipY;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();    // 자신과 자신의 자식에 있는 컴포넌트를 찾아서 배열로 리턴
    }

    protected override void MoveRight(int index)
    {
        base.MoveRight(index);

        // c#에서 숫자 앞에 0b_를 붙이면 2진수
        // c#에서 숫자 앞에 0x_를 붙이면 16진수

        int rand = Random.Range(0, 4);  // 랜덤 결과는 0 ~ 3

        // 0(0b_00), 1(0b_01), 2(0b_10), 3(0b_11)

        spriteRenderers[index].flipX = ((rand & 0b_01) != 0);   // rand의 첫번째 비트가 1이면 true 아니면 false
        spriteRenderers[index].flipY = ((rand & 0b_10) != 0);   // rand의 두번째 비트가 1이면 true 아니면 false
    }
}
