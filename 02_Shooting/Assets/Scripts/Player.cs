using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // InputManager : 기존의 유니티 입력방식
    //  장점 : 간단하다.
    //  단점 : Busy-wait이 발생할 수 밖에 없다. 인풋랙이 있을 수 있다.

    // InputSystem : 유니티의 새로운 입력 방식
    //  Event-driven 방식 적용.

    PlayerInputActions inputActions;

    // 이 스크립트가 포함된 게임 오브젝트가 생성 완료되면 호출된다.
    private void Awake()
    {
        inputActions = new PlayerInputActions();            // 인풋 액션 생성
    }

    // 이 스크립트가 포함된 게임 오브젝트가 활성화되면 호출된다.
    private void OnEnable()
    {
        inputActions.Player.Enable();                       // 활성화될 때 Player액션맵을 활성화
        inputActions.Player.Fire.performed += OnFire;       // Player액션맵의 Fire액션에 OnFire함수를 연결(눌렀을 때만 연결된 함수 실행)
        inputActions.Player.Fire.canceled += OnFire;        // Player액션맵의 Fire액션에 OnFire함수를 연결(땠을 때만 연결된 함수 실행)
    }

    // 이 스크립트가 포함된 게임 오브젝트가 비활성화되면 호출된다.
    private void OnDisable()
    {
        inputActions.Player.Fire.canceled -= OnFire;        // Player액션맵의 Fire액션에 OnFire함수를 연결해제
        inputActions.Player.Fire.performed -= OnFire;       // Player액션맵의 Fire액션에서 OnFire함수를 연결해제
        inputActions.Player.Disable();                      // Player액션맵을 비활성화
    }

    /// <summary>
    /// Fire액션이 발동했을 때 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)   // 지금 입력이 눌렀다
        {
            Debug.Log("OnFire : 눌려짐");
        }
        if(context.canceled)    // 지금 입력이 떨어졌다
        {
            Debug.Log("OnFire : 떨어짐");
        }        
    }

    // 이 스크립트가 포함된 게임 오브젝트의 첫번째 Update함수가 실행되기 직전에 호출된다.
    private void Start()
    {
        
    }

    private void Update()
    {
        // 인풋매니저 방식
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("A키가 눌러졌습니다.");
        //}

        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    Debug.Log("A키가 떨어졌습니다.");
        //}
    }

    //public void OnFire()
    //{
    //    Debug.Log("OnFire");
    //}
}
