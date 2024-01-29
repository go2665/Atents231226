using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody rigid;

    /// <summary>
    /// 이동 방향(1 : 전진, -1 : 후진, 0 : 정지)
    /// </summary>
    float moveDirection = 0.0f;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 회전방향(1 : 우회전, -1 : 좌회전, 0 : 정지)
    /// </summary>
    float rotateDirection = 0.0f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 180.0f;

    private void Awake()
    {
        //inputActions = new PlayerInputActions();
        inputActions = new();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Use.performed += OnUseInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();           
    }

    void SetInput(Vector2 input, bool isMove)
    {
        rotateDirection = input.x;
        moveDirection = input.y;
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDirection * transform.forward);     
    }

    void Rotate()
    {
        // 이번 fixedUpdate에서 추가로 회전할 회전(delta)
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection, transform.up);

        // 현재 회전에서 rotate만큼 추가로 회전
        rigid.MoveRotation(rigid.rotation * rotate);        

        // 회전을 표현하는 클래스 : Quaternion
        // Quaternion.Euler() : x, y, z 축으로 얼마만큼 회전 시킬 것인지를 파라메터로 받아서 회전을 생성하는 함수
        // Quaternion.AngleAxis() : 특정 축을 기준으로 몇 도만큼 회전 시킬지를 파라메터로 받아서 회전을 생성하는 함수
        // Quaternion.FromToRotation() : 시작 방향에서 도착 방향이 될 수 있는 회전을 생성하는 함수
        // Quaternion.Lerp() : 시작 회전에서 목표 회전으로 보간하는 함수
        // Quaternion.Slerp(): 시작 회전에서 목표 회전으로 보간하는 함수(곡선으로 보간)
        // Quaternion.LookRotation() : 특정 방향을 바라보는 회전을 만들어주는 함수
    }


    // 실습
    // 1. 플레이어는 WS키로 전진/후진을 한다.
    // 2. 플레이어는 AD키로 좌회전/우회전을 한다.
    // 3. 프레이어가 움직이면(전진/후진/좌회전/우회전) Player_Move 애니메이션이 재생된다.
    // 4. 이동 입력이 없으면 Player_Idle 애니메이션이 재생된다.
    // 5. Player_Move 애니메이션은 팔 다리가 앞뒤로 흔들린다.
    // 6. Player_Idle 애니메이션은 머리가 살짝 앞뒤로 까딱거린다.
}
