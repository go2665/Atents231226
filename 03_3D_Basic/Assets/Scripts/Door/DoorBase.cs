using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 문의 부모 클래스
/// </summary>
public class DoorBase : MonoBehaviour
{
    /// <summary>
    /// 이 문의 열쇠(사용하지 않으면 null)
    /// </summary>
    public DoorKey key = null;

    Animator animator;

    readonly int IsOpenHash = Animator.StringToHash("IsOpen");

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (key != null) // 열쇠가 있으면
        {
            key.onConsume += OnKeyUsed; 
        }
    }

    /// <summary>
    /// 문이 열릴 때 문 종류별로 따로 처리해야할 일을 기록할 가상함수
    /// </summary>
    protected virtual void OnOpen()
    {
    }

    /// <summary>
    /// 문이 닫힐 때 문 종류별로 따로 처리해야할 일을 기록할 가상함수
    /// </summary>
    protected virtual void OnClose()
    {
    }

    /// <summary>
    /// 연결된 열쇠가 소비될 때 실행일을 기록할 가상함수
    /// </summary>
    protected virtual void OnKeyUsed()
    {
    }

    /// <summary>
    /// 문을 여는 함수
    /// </summary>
    public void Open()
    {
        animator.SetBool(IsOpenHash, true);
        OnOpen();
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    public void Close()
    {
        OnClose();
        animator.SetBool(IsOpenHash, false);
    }
}
