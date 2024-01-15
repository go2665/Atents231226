using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : ObjectPool<Asteroid>
{
    protected override void OnGetObject(Asteroid component)
    {
        component.SetDestination(component.transform.position - component.transform.right);
    }
}
