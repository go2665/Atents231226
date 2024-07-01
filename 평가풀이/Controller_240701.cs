using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_240701 : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float turnSpeed = 180.0f;

    PlayerInputActions_240701 playerInputActions;
    Vector2 input;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions_240701();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.Translate(input.y * moveSpeed * Time.deltaTime * Vector3.forward);
        //transform.Translate(input.y * moveSpeed * Time.deltaTime * transform.forward, Space.World);
        transform.Rotate(input.x * turnSpeed * Time.deltaTime * Vector3.up);        
    }

}
