using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemPickup : TestBase
{
    public ItemCode code = ItemCode.Ruby;
    public uint count = 5;
    public Transform target;

#if UNITY_EDITOR

    private void Start()
    {
        target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItems(code, count, target.position, true);
    }

#endif
}
