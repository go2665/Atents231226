using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RecycleObject obj = collision.GetComponent<RecycleObject>();
        if(obj != null)
        {
            collision.gameObject.SetActive(false);  // 풀에 있는 오브젝트일 경우 비활성화
        }
        else
        {
            Destroy(collision.gameObject);  // 이 영역에 들어오는 모든 게임 오브젝트 삭제
        }

    }
}
