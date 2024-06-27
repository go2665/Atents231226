using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[CreateAssetMenu]
public class ParticleManager : ScriptableObject
{
    public enum ParticleID { Death, Jump }
    [SerializeField] private GameObject DeathParticle;
    [SerializeField] private GameObject JumpParticle;

    private Dictionary<ParticleID, List<GameObject>> _freeParticles = 
        new Dictionary<ParticleID, List<GameObject>>();

    public GameObject Get(ParticleID particle)
    {
        GameObject newGO;
        if (_freeParticles.TryGetValue(particle, out List<GameObject> free))
        {
            if (free.Count > 0)
            {
                newGO = free[0];
                free.Remove(newGO);
                newGO.SetActive(true);
                return newGO;
            }
        }
        return InstantiateParticle(ParticleIDToParticlePrefab(particle), particle);
    }

    public void ClearParticles()
    {
        _freeParticles.Clear();
    }

    public void RecycleParticle(GameObject obj, ParticleID ID)
    {
        if (_freeParticles.TryGetValue(ID, out List<GameObject> free))
        {
            obj.SetActive(false);
            free.Add(obj);
        }
    }

    private GameObject InstantiateParticle(GameObject particlePrefab, ParticleID ID)
    {
        ParticleObject particle = Instantiate(particlePrefab).GetComponent<ParticleObject>();
        particle.SetData(this, ID);

        if (!_freeParticles.ContainsKey(ID))
        {
            _freeParticles.Add(ID, new List<GameObject>());
        }

        return particle.gameObject;
    }

    private GameObject ParticleIDToParticlePrefab(ParticleID id)
    {
        switch (id)
        {
            case ParticleID.Death:
                return DeathParticle;
            case ParticleID.Jump:
                return JumpParticle;
            default:
                return DeathParticle;
        }
    }
}
