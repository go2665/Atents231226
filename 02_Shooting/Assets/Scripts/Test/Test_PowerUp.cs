using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PowerUp : TestBase
{
    Player player;

#if UNITY_EDITOR
    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Test_PowerUp();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Test_PowerDown();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetPowerUp();
    }
#endif
}
