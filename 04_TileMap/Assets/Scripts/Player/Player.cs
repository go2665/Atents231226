using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// 지금 움직이고 있는지 확인하는 변수(true면 움직인다.)
    /// </summary>
    bool isMove = false;

    // 컴포넌트들
    Rigidbody2D rigid;
    Animator animator;

    // 인풋액션
    PlayerInputAction inputActions;    

    // 애니메이터용 해시값들
    readonly int InputX_Hash = Animator.StringToHash("InputX");
    readonly int InputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("IsMove");

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // 물리 프레임마다 inputDirection방향으로 초당 speed만큼 이동
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * speed * inputDirection);
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // 입력값 받아와서
        inputDirection = context.ReadValue<Vector2>();

        // 애니메이션 조정
        animator.SetFloat(InputX_Hash, inputDirection.x);
        animator.SetFloat(InputY_Hash, inputDirection.y);
        isMove = true;
        animator.SetBool(IsMove_Hash, isMove);
    }

    private void OnStop(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        // 이동 방향을 0으로 만들고
        inputDirection = Vector2.zero;
        
        // InputX와 InputY를 변경하지 않는 이유
        // Idle애니메이션을 마지막 이동 방향으로 재생하기 위해

        isMove = false;     // 정지
        animator.SetBool(IsMove_Hash, isMove);
    }
}
