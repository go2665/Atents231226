using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeSpawn : TestBase
{
    // 겹쳐서 스폰됨
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsByType<Slime>(FindObjectsSortMode.InstanceID);
        foreach (Slime slime in slimes)
        {
            slime.Die();
        }        
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Slime[] slimes = FindObjectsByType<Slime>(FindObjectsSortMode.InstanceID);
        slimes[0].Die();
    }
}
