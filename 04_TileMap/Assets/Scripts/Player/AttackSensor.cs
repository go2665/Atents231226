using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour
{
    /// <summary>
    /// 슬라임이 내 트리거 안에 들어왔다고 알리는 델리게이트
    /// </summary>
    public Action<Slime> onEnemyEnter;

    /// <summary>
    /// 슬라임이 내 트리거 밖으로 나갔다고 알리는 델리게이트
    /// </summary>
    public Action<Slime> onEnemyExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if(slime != null )
        {
            onEnemyEnter?.Invoke(slime);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Slime slime = collision.GetComponent<Slime>();
        if (slime != null)
        {
            onEnemyExit?.Invoke(slime);
        }
    }
}
