using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Enemies : TestBase
{
    Transform spawnPoint;

    private void Start()
    {
        spawnPoint = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBonus(spawnPoint.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 커브 적
        Factory.Instance.GetCurve(spawnPoint.position);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 보스
    }
}
