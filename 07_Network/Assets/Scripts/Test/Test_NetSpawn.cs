using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_NetSpawn : TestBase
{
    public GameObject orb;
    Transform fire;

    private void Start()
    {
        fire = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(orb, fire.position, fire.rotation);
    }
}
