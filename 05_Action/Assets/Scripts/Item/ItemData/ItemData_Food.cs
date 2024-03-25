using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Food", menuName = "Scriptable Object/Item Data - Food", order = 2)]
public class ItemData_Food : ItemData, IConsumable
{
    [Header("음식 아이템 데이터")]
    public float tickRegen = 1.0f;
    public float tickInterval = 1.0f;
    public uint tickCount = 1;

    public void Consume(GameObject target)
    {
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            // target은 HP를 가지는 대상이다.
            health.HealthRegenerateByTick(tickRegen, tickInterval, tickCount);
        }
    }
}