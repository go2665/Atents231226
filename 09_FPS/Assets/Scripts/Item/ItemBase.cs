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
         if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                OnItemConsum(player);

                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// 아이템을 먹었을 때 실행되는 함수
    /// </summary>
    /// <param name="player">아이템을 먹은 플레이어</param>
    protected virtual void OnItemConsum(Player player)
    {
    }
}

// 총기 아이템 획득하기
// 1. 라이플과 샷건 아이템 만들기
//  1.1. 획득하면  획득시 무기가 변경됨
// 2. 라이플과 샷건을 사용할 때 총알이 다 떨어지면 기본총으로 변경

// 아이템도 풀에 추가하기