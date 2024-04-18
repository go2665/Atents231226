using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Log : TestBase
{
    public Logger logger;
    int count = 0;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        logger.Log($"Test - {count++}");
    }
}
