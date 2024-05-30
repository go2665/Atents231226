using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_08_Gun : TestBase
{
    public Revolver revoler;
    public Shotgun shotgun;
    public AssaultRifle assaultRifle;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        revoler.Test_Fire();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        revoler.Reload();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        shotgun.Test_Fire();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        assaultRifle.Test_Fire(!context.canceled);        
    }
}
