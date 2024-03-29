using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : RecycleObject
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(LifeOver(ps.main.duration));
    }
}
