using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_12_Item : TestBase
{
    Transform spawn;

    private void Start()
    {
        spawn = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAssaultRifleItem(spawn.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetShotgunItem(spawn.position);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHealPackItem(spawn.position);
    }
}
