using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 이동 입력을 전달하는 델리게이트(파라메터 : 이동방향, 누른상황인지 아닌지(true면 눌렀다))
    /// </summary>
    public Action<Vector2, bool> onMove;

    /// <summary>
    /// 이동 모드 변경 입력을 전달하는 델리게이트
    /// </summary>
    public Action onMoveModeChange;

    /// <summary>
    /// 공격 입력을 전달하는 델리게이트
    /// </summary>
    public Action onAttack;

    /// <summary>
    /// 아이템을 줍는 입력을 전달하는 델리게이트
    /// </summary>
    public Action onItemPickUp;

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
        inputActions.Player.Pickup.performed += OnPickup;
    }

    private void OnDisable()
    {
        inputActions.Player.Pickup.performed -= OnPickup;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);        
    }

    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onAttack?.Invoke();        
    }

    private void OnPickup(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onItemPickUp?.Invoke();
    }
}