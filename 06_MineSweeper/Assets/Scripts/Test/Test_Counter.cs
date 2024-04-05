using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Counter : TestBase
{
    public int flagCount = 5;
    public GameManager.GameState gameState;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //GameManager.Instance.onFlagCountChange(flagCount);
        GameManager.Instance.Test_SetFlagCount(flagCount);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Test_StateChage(gameState);
    }
}

// 실습
// 1. Timer클래스 완성하기
// 2. CounterBase 클래스 만들기