using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_End : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Die();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.GameClear();
    }
}
