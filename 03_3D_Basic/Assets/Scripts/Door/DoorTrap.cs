using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorTrap : DoorManual
{
    ParticleSystem ps;
    List<IAlive> alives;

    protected override void Awake()
    {
        base.Awake();
        
        Transform child = transform.GetChild(3);
        ps = child.GetComponent<ParticleSystem>();

        alives = new List<IAlive>(4);
    }

    protected override void OnOpen()
    {
        ps.Stop();
        ps.Play();   
        foreach (IAlive alive in alives)
        {
            alive.Die();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        IAlive alive = other.GetComponent<IAlive>();
        if(alive != null)
        {
            alives.Add(alive);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        IAlive alive = other.GetComponent<IAlive>();
        if (alive != null)
        {
            alives.Remove(alive);
        }
    }
}
