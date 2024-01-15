using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AsteroidMini : TestBase
{
    public Transform target;

    private void Start()
    {
        target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        AsteroidMini mini = Factory.Instance.GetAsteroidMini();
        mini.Direction = target.position - mini.transform.position;
    }
}
