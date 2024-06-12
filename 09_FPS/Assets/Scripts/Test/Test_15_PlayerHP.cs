using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_15_PlayerHP : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 1. Player HP 감소
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 2. Player HP 증가
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 적 안움직이게 만들기
    }
}

// HealthPoint 완성하기
// BloodOverlay 완성하기
// HitDirection 완성하기
// Player.OnAttacked 완성하기
