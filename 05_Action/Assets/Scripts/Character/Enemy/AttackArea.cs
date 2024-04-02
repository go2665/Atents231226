using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 공격을 2번 맞는 버그수정을 위해 필수
/// </summary>
public class AttackArea : MonoBehaviour
{
    /// <summary>
    /// 플레이어가 들어왔을 때 실행되는 델리게이트
    /// </summary>
    public Action<IBattler> onPlayerIn;

    /// <summary>
    /// 플레이어가 나갔을 때 실행되는 델리게이트
    /// </summary>
    public Action<IBattler> onPlayerOut;

    /// <summary>
    /// 공격 범위를 결정하는 컬라이더
    /// </summary>
    public SphereCollider attackArea;   // 실행전에도 기즈모로 표시하기 위해 public으로 설정한 후 인스팩터 창에서 지정

    private void Awake()
    {
        attackArea = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어왔으면
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>(); 
            onPlayerIn?.Invoke(target);     // 플레이어가 들어왔음을 알림
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IBattler target = other.GetComponent<IBattler>();  
            onPlayerOut?.Invoke(target);    // 플레이어가 나갔음을 알림
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackArea != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, attackArea.radius, 5);   // 공격 범위 그리기
        }
    }
#endif
}
