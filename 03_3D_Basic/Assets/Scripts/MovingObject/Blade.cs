using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : WaypointUser
{
    public float spinSpeed = 720.0f;
    Transform bladeMesh;

    private void Awake()
    {
        bladeMesh = transform.GetChild(0);
    }

    private void Update()
    {
        bladeMesh.Rotate(Time.deltaTime * spinSpeed * Vector3.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IAlive live = collision.gameObject.GetComponent<IAlive>();
        if(live != null)
        {
            live.Die();
        }
    }
}

// 1. Blade : 웨이포인트 사용했을 때 문제점 수정
// 2. Waypoints : GetNextWaypoint 함수 구현하기
// 3. WaypointUser : Target 프로퍼티 구현, IsArrived 프로퍼티 구현, OnMove 함수 구현
