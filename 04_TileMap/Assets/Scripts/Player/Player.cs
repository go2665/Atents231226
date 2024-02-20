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
    /// 현재 이동 속도
    /// </summary>
    float currentSpeed = 3.0f;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// 지금 움직이고 있는지 확인하는 변수(true면 움직인다.)
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 현재 남아있는 공격 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// 공격 쿨타임이 다 되었는지 확인하기 위한 프로퍼티 
    /// </summary>
    bool IsAttackReady => currentAttackCoolTime < 0.0f;

    // 컴포넌트들
    Rigidbody2D rigid;
    Animator animator;

    // 인풋액션
    PlayerInputAction inputActions;    

    // 애니메이터용 해시값들
    readonly int InputX_Hash = Animator.StringToHash("InputX");
    readonly int InputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("IsMove");
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputAction();

        currentSpeed = speed;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // 물리 프레임마다 inputDirection방향으로 초당 currentSpeed만큼 이동
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);
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

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if( IsAttackReady ) // 공격 쿨타임이 다 되었으면
        {
            animator.SetTrigger(Attack_Hash);       // 애니메이션 재생
            currentAttackCoolTime = attackCoolTime; // 쿨타임 초기화
            currentSpeed = 0.0f;                    // 이동 정지
        }
    }

    /// <summary>
    /// 이동 속도를 원래대로 되돌리는 함수
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }
}

