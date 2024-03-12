using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Action<Vector2, bool> onMove;
    public Action onMoveModeChange;
    public Action onAttack;

    // 입력용 인풋 액션
    PlayerInputActions inputActions;

    private void Awake()
    {        
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);        
    }

    /// <summary>
    /// 이동 모드 변경용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onAttack?.Invoke();        
    }
}