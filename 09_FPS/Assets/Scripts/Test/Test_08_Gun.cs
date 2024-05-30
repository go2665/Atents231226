using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_Gun : TestBase
{
    public GunBase gun;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        gun.Test_Fire();
    }
}
