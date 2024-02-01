using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoOneWay : DoorAuto
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // other는 플레이어
            Vector3 playerToDoor = transform.position - other.transform.position;   // 플레이어에서 문으로 향하는 방향 벡터

            float angle = Vector3.Angle(transform.forward, playerToDoor);   
            if(angle > 90.0f)   // 사이각이 90도보다 크면 플레이어가 문 앞에 있다.
            {
                Open();
            }
        }
    }
}
