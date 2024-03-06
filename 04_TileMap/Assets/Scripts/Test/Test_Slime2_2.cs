using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime2_2 : TestBase
{
#if UNITY_EDITOR
    public Slime slime;
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        slime.TestShader(1);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        slime.TestShader(2);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        slime.TestShader(3);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        slime.TestShader(4);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        slime.TestShader(5);
    }
#endif
}
