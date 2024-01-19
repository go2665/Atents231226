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
        // 보스(총알)
        Factory.Instance.GetBossBullet(spawnPoint.position);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // 보스(미사일)
        Factory.Instance.GetBossMisslie(spawnPoint.position);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // 보스(본체)
        Factory.Instance.GetBoss(spawnPoint.position);
    }
}
