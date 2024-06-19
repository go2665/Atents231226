using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_18_CellCorner : TestBase
{
    Direction dir = Direction.North;

    protected override void OnTest1(InputAction.CallbackContext context)
    {        
        dir++;
        Debug.Log(dir);
    }
}
