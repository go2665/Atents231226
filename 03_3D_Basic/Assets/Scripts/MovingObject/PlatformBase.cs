using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : WaypointUser
{
    /// <summary>
    /// 이동할 때 실행될 델리게이트
    /// </summary>
    public Action<Vector3> onMove;

    protected override void OnMove()
    {
        base.OnMove();
        onMove?.Invoke(moveDelta);
    }
}
