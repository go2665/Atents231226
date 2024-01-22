using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    public int seed = -1;
    const int allRandom = -1;
    TestInputActions inputActions;

    private void Awake()
    {
        inputActions = new TestInputActions();

        if( seed != allRandom ) // -1일 때는 완전 랜덤. 그 외에 값은 시드로 설정됨
        {
            UnityEngine.Random.InitState(seed);
        }
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += OnTest1;
        inputActions.Test.Test2.performed += OnTest2;
        inputActions.Test.Test3.performed += OnTest3;
        inputActions.Test.Test4.performed += OnTest4;
        inputActions.Test.Test5.performed += OnTest5;
        inputActions.Test.LClick.performed += OnTestLClick;
        inputActions.Test.RClick.performed += OnTestRClick;
    }

    private void OnDisable()
    {
        inputActions.Test.RClick.performed -= OnTestRClick;
        inputActions.Test.LClick.performed -= OnTestLClick;
        inputActions.Test.Test5.performed -= OnTest5;
        inputActions.Test.Test4.performed -= OnTest4;
        inputActions.Test.Test3.performed -= OnTest3;
        inputActions.Test.Test2.performed -= OnTest2;
        inputActions.Test.Test1.performed -= OnTest1;
        inputActions.Test.Disable();
    }

    protected virtual void OnTestRClick(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTestLClick(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest5(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest4(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest3(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest2(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnTest1(InputAction.CallbackContext context)
    {
    }
}
