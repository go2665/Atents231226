using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManualKeyUse : DoorManual
{
    protected override void OnKeyUsed()
    {
        Open();
    }
}
