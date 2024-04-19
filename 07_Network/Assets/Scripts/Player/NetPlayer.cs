using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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
    NetworkVariable<float> netRotate = new NetworkVariable<float>(0.0f);

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

    /// <summary>
    /// 애니메이션 상태 처리용 네트워크 변수
    /// </summary>
    NetworkVariable<AnimationState> netAnimState = new NetworkVariable<AnimationState>();

    /// <summary>
    /// 채팅용 네트워크 변수
    /// </summary>
    NetworkVariable<FixedString512Bytes> chatString = new NetworkVariable<FixedString512Bytes>();


    // 컴포넌트 들
    CharacterController controller;
    Animator animator;
    PlayerInputActions inputActions;

    // 유니티 이벤트 함수들 ----------------------------------------------------------------------------
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputActions();

        netAnimState.OnValueChanged += OnAnimStateChange;
        chatString.OnValueChanged += OnChatRecieve;
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

    private void Update()
    {
        if (netMoveDir.Value != 0.0f)
        {
            controller.SimpleMove(netMoveDir.Value * transform.forward);
        }
        transform.Rotate(0, netRotate.Value * Time.deltaTime, 0, Space.World);
    }

    // 입력 처리용 함수들 ------------------------------------------------------------------------------
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        float moveInput = context.ReadValue<float>();   // 키보드라 -1, 0, 1 중 하나
        SetMoveInput(moveInput);
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        float rotateInput = context.ReadValue<float>(); // 키보드라 -1, 0, 1 중 하나
        SetRotateInput(rotateInput);
    }

    // 기타 ------------------------------------------------------------------------------------------
    void SetMoveInput(float moveInput)
    {        
        if(IsOwner)
        {
            float moveDir = moveInput * moveSpeed;
            if(IsServer)
            {
                netMoveDir.Value = moveDir;
            }
            else
            {
                MoveRequestServerRpc(moveDir);
            }

            // 애니메이션 변경
            if (moveDir > 0.001f)
            {
                state = AnimationState.Walk;
            }
            else if (moveDir < -0.001f)
            {
                state = AnimationState.BackWalk;
            }
            else
            {
                state = AnimationState.Idle;
            }
            if (state != netAnimState.Value)
            {
                if (IsServer)
                {
                    netAnimState.Value = state;
                }
                else
                {
                    UpdateAnimStateServerRpc(state);
                }
            }
        }
        
    }

    void SetRotateInput(float rotateInput)
    {
        if(IsOwner)
        {
            float rotate = rotateInput * rotateSpeed;
            if(IsServer)
            {
                netRotate.Value = rotate;
            }
            else
            {
                RotateRequestServerRpc(rotate);
            }
        }
    }

    private void OnAnimStateChange(AnimationState previousValue, AnimationState newValue)
    {
        animator.SetTrigger(newValue.ToString());
    }


    // 채팅 ----------------------------------------------------------------------------------

    /// <summary>
    /// 채팅을 보내는 함수
    /// </summary>
    /// <param name="message"></param>
    public void SendChat(string message)
    {
        if(IsServer)
        {
            chatString.Value = message;
        }
        else
        {
            RequestChatServerRpc(message);
        }
    }

    /// <summary>
    /// 채팅을 받았을 때 처리하는 함수
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void OnChatRecieve(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        GameManager.Instance.Log(newValue.ToString());
    }


    // 서버 Rpc들 -----------------------------------------------------------------------------------------------

    [ServerRpc]
    void MoveRequestServerRpc(float move)
    {
        netMoveDir.Value = move;
    }

    [ServerRpc]
    void RotateRequestServerRpc(float rotate)
    {
        netRotate.Value = rotate;
    }

    [ServerRpc]
    void UpdateAnimStateServerRpc(AnimationState state)
    {
        netAnimState.Value = state;
    }

    [ServerRpc]
    void RequestChatServerRpc(FixedString512Bytes message)
    {
        chatString.Value = message;
    }

}

// 실습
// 1. rotate도 네트워크 변수로 적용하기
// 2. 네트워크로 애니메이션 되게 만들기(netAnimState 네트워크 변수 만들어서 상태 변환 처리하기)