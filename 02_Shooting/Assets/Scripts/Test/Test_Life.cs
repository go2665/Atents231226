using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Life : TestBase
{
    public PoolObjectType type;
    Transform spawn;

#if UNITY_EDITOR
    private void Start()
    {
        spawn = transform.GetChild(0);

        GameManager.Instance.Player.onLifeChange += TestLife;

    }

    private void TestLife(int life)
    {
        Debug.Log($"Player Life : {life}");
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetObject(type, spawn.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Test_Die();
    }
#endif
}
