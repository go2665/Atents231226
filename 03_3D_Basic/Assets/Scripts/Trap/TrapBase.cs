using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnTrapActivate(other.gameObject);
    }

    protected virtual void OnTrapActivate(GameObject target)
    {
    }


    // 1. TrapSpike : 밟으면 바닥에서 가시가 올라왔다가 내려가면서 플레이어를 죽인다.
    // 2. TrapPush : 밟으면 발판이 튀어오르며 플레이어를 밀어낸다.
    // 3. TrapFire : 밟으면 바닥에서 불이 올라와 플레이어를 죽인다.
    // 4. TrapSlow : 밟으면 일정시간 동안 플레이어의 이동속도가 내려간다.
}
