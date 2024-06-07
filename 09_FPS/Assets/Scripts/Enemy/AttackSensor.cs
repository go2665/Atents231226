using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    public Action<GameObject> onSensorTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onSensorTriggered?.Invoke(other.gameObject);
        }
    }
}
