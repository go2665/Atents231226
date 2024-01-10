using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : TestBase
{
    public BulletPool pool;

    private void Start()
    {
        pool.Initialize();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Bullet bullet = pool.GetObject();
    }
}
