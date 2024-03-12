using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
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
}
