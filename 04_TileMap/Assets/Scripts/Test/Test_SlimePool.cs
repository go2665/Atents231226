using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimePool : TestBase
{
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetSlime(new(Random.Range(-8,8), Random.Range(-4,4)));
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsOfType<Slime>(false);
        foreach(Slime slime in slimes)
        {
            slime.TestDie();
        }
    }
#endif
}
