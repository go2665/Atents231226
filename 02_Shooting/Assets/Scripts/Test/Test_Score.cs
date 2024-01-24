using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Score : TestBase
{
    Player player;

#if UNITY_EDITOR
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.AddScore(10);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.AddScore(100);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.AddScore(1000);
    }
#endif
}
