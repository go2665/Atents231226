using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class NetPlayer : NetworkBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 90.0f;

    /// <summary>
    /// 마지막 입력으로 인한 이동 방향(전진, 정지, 후진), 네트워크에서 공유되는 변수
    /// </summary>
    NetworkVariable<float> netMoveDir = new NetworkVariable<float>(0.0f);

    /// <summary>
    /// 마지막 입력으로 인한 회전 방향(좌회전, 정지, 우회전)
    /// </summary>
    NetworkVariable<float> rotate = new NetworkVariable<float>(0.0f);

    /// <summary>
    /// 애니메이션 상태
    /// </summary>
    enum AnimationState
    {
        Idle,       // 대기
        Walk,       // 걷기
        BackWalk,   // 뒤로걷기
        None        // 초기값용
    }

    /// <summary>
    /// 현재 애니메이션 상태
    /// </summary>
    AnimationState state = AnimationState.None;

    ///// <summary>
    ///// 애니메이션 상태 설정 및 확인용 프로퍼티
    ///// </summary>
    //AnimationState State
    //{
    //    get => state; 
    //    set
    //    {
    //        if (value != state)
    //        {
    //            state = value;
    //            animator.SetTrigger(state.ToString());
    //        }
    //    }
    //}

    // 컴포넌트 들
    CharacterController controller;
    Animator animator;
    PlayerInputActions inputActions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputActions();

        //netMoveDir.OnValueChanged += aaa;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.MoveForward.performed += OnMoveInput;
        inputActions.Player.MoveForward.canceled += OnMoveInput;
        inputActions.Player.Rotate.performed += OnRotate;
        inputActions.Player.Rotate.canceled += OnRotate;
    }

    private void OnDisable()
    {
        inputActions.Player.Rotate.canceled -= OnRotate;
        inputActions.Player.Rotate.performed -= OnRotate;
        inputActions.Player.MoveForward.canceled -= OnMoveInput;
        inputActions.Player.MoveForward.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();   // 키보드라 -1, 0, 1 중 하나
        SetMoveInput(moveInput);
    }

    void SetMoveInput(float moveInput)
    {
        float moveDir = moveInput * moveSpeed;
        
        if( NetworkManager.Singleton.IsServer )
        {
            netMoveDir.Value = moveDir;
        }
        else if(IsOwner)
        {
            MoveRequestServerRpc(moveDir);
        }

        //if(moveDir > 0.001f)
        //{
        //    State = AnimationState.Walk;
        //}
        //else if(moveDir < -0.001f)
        //{
        //    State = AnimationState.BackWalk;
        //}
        //else
        //{
        //    State = AnimationState.Idle;
        //}
    }

    [ServerRpc]
    void MoveRequestServerRpc(float move)
    {
        netMoveDir.Value = move;
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>(); // 키보드라 -1, 0, 1 중 하나


        //rotate = rotateInput * rotateSpeed;
    }

    private void Update()
    {
        if(netMoveDir.Value != 0.0f)
        {
            controller.SimpleMove(netMoveDir.Value * transform.forward);
        }
        //transform.Rotate(0, rotate * Time.deltaTime, 0, Space.World);
    }
}

// 실습
// 1. rotate도 네트워크 변수로 적용하기
// 2. netAnimState 네트워크 변수 만들어서 상태 변환 처리하기