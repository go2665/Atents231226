using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePool : ObjectPool<Curve>
{
    protected override void OnGetObject(Curve component)
    {
        component.RefreashRotateDirection();
    }
}
