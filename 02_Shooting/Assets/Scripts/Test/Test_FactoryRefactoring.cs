using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_FactoryRefactoring : TestBase
{
    public PoolObjectType objectType;

    [Range(0,360.0f)]
    public float angle = 0;

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
        Factory.Instance.GetObject(objectType, spawnPoint.position, new Vector3(0, 0, angle));
    }
#endif
}
