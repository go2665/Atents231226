using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Effect.Enable();
        inputActions.Effect.PointerMove.performed += OnPointerMove;
    }

    private void OnDisable()
    {
        inputActions.Effect.PointerMove.performed -= OnPointerMove;
        inputActions.Effect.Disable();
    }

    private void OnPointerMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();    // 마우스의 스크린 좌표 받아오기
        //Debug.Log(mousePos);
        mousePos.z = 10.0f;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);  // 스크린 좌표를 월드 좌표로 바꾸기
        transform.position = target;
    }
}
