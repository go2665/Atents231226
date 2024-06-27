using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    private ParticleManager _manager;
    private ParticleManager.ParticleID _id;

    public void SetData(ParticleManager manager, ParticleManager.ParticleID id)
    {
        _manager = manager;
        _id = id;
    }

    private void OnDisable()
    {
        Recycle();
    }

    private void Recycle()
    {
        _manager?.RecycleParticle(gameObject, _id);
    }
}
