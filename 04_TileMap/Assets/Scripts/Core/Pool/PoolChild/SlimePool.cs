using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool<Slime>
{
    protected override void OnGenerateObject(Slime comp)
    {
        comp.Pool = comp.transform.parent;  // pool 설정
    }
}
