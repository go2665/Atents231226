using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class AsteroidMiniPool : ObjectPool<AsteroidMini>
{
    protected override void OnGetObject(AsteroidMini component)
    {
        component.Direction = -component.transform.right;
    }
}
