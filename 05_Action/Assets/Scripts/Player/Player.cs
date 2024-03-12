using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float currentSpeed = 0.0f;

    /// <summary>
    /// 이동 모드
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // 걷기 모드
        Run         // 달리기 모드
    }

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Run;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;    // 상태 변경
            if (currentSpeed > 0.0f)     // 이동 중인지 아닌지 확인
            {
                // 이동 중이면 모드에 맞게 속도와 애니메이션 변경
                MoveSpeedChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y는 무조건 바닥 높이


    /// <summary>
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 캐릭터 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    /// <summary>
    /// 무기 장비할 트랜스폼
    /// </summary>
    Transform weaponParent;

    /// <summary>
    /// 방패 장비할 트랜스폼
    /// </summary>
    Transform shiledParent;

    // 애니메이터용 해시값 및 상수
    readonly int Speed_Hash = Animator.StringToHash("Speed");
    const float AnimatorStopSpeed = 0.0f;
    const float AnimatorWalkSpeed = 0.3f;
    const float AnimatorRunSpeed = 1.0f;
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    // 컴포넌트들
    Animator animator;
    CharacterController characterController;
    PlayerInputController inputController;

    private void Awake()
    {
        Transform child = transform.GetChild(2);    // root
        child = child.GetChild(0);                  // pelvis
        child = child.GetChild(0);                  // spine1
        child = child.GetChild(0);                  // spine2

        Transform spine3 = child.GetChild(0);       // spine3
        weaponParent = spine3.GetChild(2);          // clavicle_r
        weaponParent = weaponParent.GetChild(1);    // upperarm_r
        weaponParent = weaponParent.GetChild(0);    // lowerarm_r
        weaponParent = weaponParent.GetChild(0);    // hand_r
        weaponParent = weaponParent.GetChild(2);    // weapon_r

        shiledParent = spine3.GetChild(1);          // clavicle_l
        shiledParent = shiledParent.GetChild(1);    // upperarm_l
        shiledParent = shiledParent.GetChild(0);    // lowerarm_l
        shiledParent = shiledParent.GetChild(0);    // hand_l
        shiledParent = shiledParent.GetChild(2);    // weapon_l

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        inputController = GetComponent<PlayerInputController>();

        inputController.onMove += OnMoveInput;
        inputController.onMoveModeChange += OnMoveModeChageInput;
        inputController.onAttack += OnAttackInput;
    }

    private void Update()
    {
        characterController.Move(Time.deltaTime * currentSpeed * inputDirection);   // 좀 더 수동
        //characterController.SimpleMove(currentSpeed * inputDirection);            // 좀 더 자동

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  // 목표 회전으로 변경
    }

    private void OnMoveInput(Vector2 input, bool isPress)
    {
        inputDirection.x = input.x;     // 입력 방향 저장
        inputDirection.y = 0;
        inputDirection.z = input.y;

        if (isPress)
        {
            // 눌려진 상황(입력을 시작한 상황)

            // 입력 방향 회전 시키기
            Quaternion camY = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0); // 카메라의 y회전만 따로 추출
            inputDirection = camY * inputDirection;     // 입력 방향을 카메라의 y회전과 같은 정도로 회전 시키기
            targetRotation = Quaternion.LookRotation(inputDirection);   // 목표 회전 저장

            // 이동 모드 변경
            MoveSpeedChange(CurrentMoveMode);
        }
        else
        {
            // 입력을 끝낸 상황
            currentSpeed = 0.0f;    // 정지 시키기
            animator.SetFloat(Speed_Hash, AnimatorStopSpeed);
        }
    }

    private void OnMoveModeChageInput()
    {
        if (CurrentMoveMode == MoveMode.Walk)
        {
            CurrentMoveMode = MoveMode.Run;
        }
        else
        {
            CurrentMoveMode = MoveMode.Walk;
        }
    }

    private void OnAttackInput()
    {
        animator.SetTrigger(Attack_Hash);
    }

    /// <summary>
    /// 모드에 따라 이동 속도를 변경하는 함수
    /// </summary>
    /// <param name="mode">설정된 모드</param>
    void MoveSpeedChange(MoveMode mode)
    {
        switch (mode) // 이동 모드에 따라 속도와 애니메이션 변경
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                animator.SetFloat(Speed_Hash, AnimatorWalkSpeed);
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                animator.SetFloat(Speed_Hash, AnimatorRunSpeed);
                break;
        }
    }

    public void ShowWeaponAndShield(bool isShow = true)
    {
        weaponParent.gameObject.SetActive(isShow);
        shiledParent.gameObject.SetActive(isShow);
    }
}
