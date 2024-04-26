using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Ship : TestBase
{
    public ShipType shipType = ShipType.Carrier;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ShipManager.Instance.MakeShip(shipType, null);
    }

}
