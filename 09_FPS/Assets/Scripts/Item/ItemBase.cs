using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// 1초에 회전하는 속도
    /// </summary>
    public float spinSpeed = 360.0f;

    /// <summary>
    /// 회전시킬 메시의 트랜스폼
    /// </summary>
    Transform meshTransform;

    private void OnTriggerEnter(Collider other)
    {
        // OnItemConsum 실행
    }

    /// <summary>
    /// 아이템을 먹었을 때 실행되는 함수
    /// </summary>
    /// <param name="player">아이템을 먹은 플레이어</param>
    protected virtual void OnItemConsum(Player player)
    {
    }
}
