using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_GunChange : TestBase
{
    public Player player;

    public GunType gunType = GunType.Revolver;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.GunChange(gunType);
    }
}
