using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 입력을 받으면 알리는 역할만하는 클래스
/// </summary>
public class InputController : MonoBehaviour
{
    public Action<Vector2> onMouseMove;
    public Action<Vector2> onMouseClick;
    public Action<float> onMouseWheel;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Click.performed += OnClick;
        inputActions.Player.Wheel.performed += OnWheel;
    }

    private void OnDisable()
    {
        inputActions.Player.Wheel.performed -= OnWheel;
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // 어디로 움직였는지 알림
        onMouseMove?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnClick(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        // 어느 위치를 클릭했는지 알림
        onMouseClick?.Invoke(Mouse.current.position.ReadValue());
    }

    private void OnWheel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // 휠을 얼마만큼 돌렸는지 알림
        onMouseWheel?.Invoke(context.ReadValue<float>());
    }

    /// <summary>
    /// 기존에 바인딩 된 함수들을 모두 제거
    /// </summary>
    public void ResetBind()
    {
        onMouseMove = null;
        onMouseClick = null;
        onMouseWheel = null;
    }
}
