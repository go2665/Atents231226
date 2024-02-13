using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 수동문을 열고 닫는 스위치
/// </summary>
public class UseSwitch : MonoBehaviour, IInteracable
{
    /// <summary>
    /// 스위치의 상태
    /// </summary>
    enum State
    {
        Off = 0,    // 스위치가 꺼진 상태
        On,         // 스위치가 켜진 상태
    }

    /// <summary>
    /// 스위치의 현재 상태
    /// </summary>
    State state = State.On;

    /// <summary>
    /// target이 될 IInteracable
    /// </summary>
    IInteracable target;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 애니메이터용 해시
    /// </summary>
    readonly int IsUseHash = Animator.StringToHash("IsUse");

    /// <summary>
    /// 재사용 쿨타임
    /// </summary>
    public float coolTime = 0.5f;

    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float currentCoolTime = 0;

    /// <summary>
    /// 사용 가능 여부. 쿨타임이 0 미만일 때 사용 가능
    /// </summary>
    public bool CanUse => currentCoolTime < 0.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = transform.parent.GetComponent<IInteracable>();
        if(target == null)
        {
            Debug.LogWarning($"{gameObject.name}에게 사용할 오브젝트가 없습니다.");  // 오브젝트가 없으면 경고 출력
        }
    }

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    /// <summary>
    /// 스위치 사용
    /// </summary>
    public void Use()
    {
        if(target != null && CanUse)  // 조작할 오브젝트가 있어야 한다. 그리고 사용 가능할 때만 사용
        {
            // 스위치 문에서는 애니메이션만 처리
            switch (state)
            {
                case State.Off:
                    // 스위치를 켜는 상황
                    animator.SetBool(IsUseHash, true);      // 스위치 애니메이션 재생
                    state = State.On;                       // 상태 변경
                    break;
                case State.On:
                    // 스위치를 끄려는 상황
                    animator.SetBool(IsUseHash, false);     // 스위치 애니메이션 재생
                    state = State.Off;                      // 상태 변경
                    break;
            }

            target.Use();   // 대상을 사용하기
        }
    }
}