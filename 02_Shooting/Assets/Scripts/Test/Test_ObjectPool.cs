using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : TestBase
{
    public BulletPool pool;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        pool.Initialize();
    }
}
