using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : TestBase
{

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 내 캐릭터의 얼굴부분이 안빛나게 만들기

        GameManager.Instance.PlayerDeco.IsEffectOn = false;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 내 캐릭터의 얼굴부분이 빛나게 만들기
        GameManager.Instance.PlayerDeco.IsEffectOn = true;
    }
}
