using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public float moveSpeed = 20.0f;

    [Networked]
    TickTimer Life { get; set; }


    public void Init()
    {
        Life = TickTimer.CreateFromSeconds(Runner, 5.0f);   // life는 5초를 카운팅한다.
    }

    public override void FixedUpdateNetwork()
    {
        if(Life.Expired(Runner))    // life의 시간이 만료되면
        {
            Runner.Despawn(Object); // 오브젝트 디스폰            
        }
        else
        {
            transform.position += Runner.DeltaTime * moveSpeed * transform.forward; // 앞쪽으로 계속 이도
        }
    }

}
