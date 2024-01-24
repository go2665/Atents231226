using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Singleton : TestBase
{
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Singleton s = Singleton.Instance;
    }
#endif
}