using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_10_Turn : Test_09_PlayerBase
{
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.AutoAttack();
    }
}
