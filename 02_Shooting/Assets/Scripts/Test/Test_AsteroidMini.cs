using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AsteroidMini : TestBase
{
    Transform target;
    Transform spawnPoint;

#if UNITY_EDITOR
    private void Start()
    {
        target = transform.GetChild(0);
        spawnPoint = transform.GetChild(1);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        AsteroidMini mini = Factory.Instance.GetAsteroidMini();
        mini.Direction = target.position - mini.transform.position;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAsteroid(spawnPoint.position);
    }
#endif
}
