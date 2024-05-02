using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_09_PlayerBase : TestBase
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    public PlayerBase user;
    public PlayerBase enemy;

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        // 보드 공격하기(유저, 적 상관없음)
    }

    protected override void OnTestRClick(InputAction.CallbackContext context)
    {
        // 그 위치에 배치된 배를 배치 해제
    }
}
