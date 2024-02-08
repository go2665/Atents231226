using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSlow : TrapBase
{
    /// <summary>
    /// 밟았을 때 슬로우 지속시간
    /// </summary>
    public float slowDuration = 5.0f;

    /// <summary>
    /// 밟았을 떄 느려지는 비율( 0.5 = 50% )
    /// </summary>
    [Range(0.1f, 2.0f)]
    public float slowRatio = 0.5f;

    ParticleSystem ps;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();      // 이팩트 재생
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.SetSpeedModifier(slowRatio); // 대상이 플레이어면 속도 조정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            StopAllCoroutines();
            StartCoroutine(RestoreSpeed(player));   // 트리거에서 나간 것이 플레이어이면 slowDuration후에 속도 복구
        }
    }

    IEnumerator RestoreSpeed(Player player)
    {
        yield return new WaitForSeconds(slowDuration);
        player.RestoreMoveSpeed();
    }
}
