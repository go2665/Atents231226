using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy_240325 : MonoBehaviour
{
    public float sightRange = 10.0f;
        
    Transform[] waypoints;
    int index = 0;
    Transform target;

    NavMeshAgent agent;
    SphereCollider sphereCollider;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        sphereCollider.radius = sightRange;

        Transform way = GameObject.Find("Waypoints").transform;
        
        waypoints = new Transform[way.childCount];
        for(int i = 0; i < way.childCount; i++)
        {
            waypoints[i] = way.GetChild(i);
        }

        agent.SetDestination(waypoints[index].position);

    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
    }

    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
            return;
        }
        
        if(!agent.pathPending && agent.remainingDistance < agent.stoppingDistance )
        {
            // 웨이포인트 지점에 도착
            GoNext();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = null;
            ReturnPatrol();
        }
    }

    void GoNext()
    {
        index++;
        index %= waypoints.Length;
        agent.SetDestination(waypoints[index].position);        
    }

    void ReturnPatrol()
    {
        agent.SetDestination(waypoints[index].position);
    }
}
