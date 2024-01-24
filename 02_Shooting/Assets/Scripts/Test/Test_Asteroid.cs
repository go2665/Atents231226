using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Asteroid : TestBase
{
    public Asteroid asteroid;
    public Transform target;
#if UNITY_EDITOR
    void Start()
    {
        target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        asteroid.SetDestination(target.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetAsteroid();
    }
#endif
}
