using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyBase : TestBase
{
    public PoolObjectType objectType;
    Transform spawn;

    private void Start()
    {
        spawn = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemyWave(spawn.position);
        // 생성시 회전 처리
    }
}
