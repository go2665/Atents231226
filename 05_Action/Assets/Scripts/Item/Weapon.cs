using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    CapsuleCollider bladeCollider;
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        bladeCollider = GetComponent<CapsuleCollider>();
    }

    public void EffectEnable(bool enable)
    {
        if(enable)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }

    public void BladeColliderEnable(bool isEnable)
    {
        bladeCollider.enabled = isEnable;
    }
}
