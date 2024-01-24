using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Test_Delegate : TestBase
{
    // Delegate(델리게이트)
    //  - 함수를 저장할 수 있는 변수 타입
    //  - 함수 체인(chain)이 가능
    //  - 어떤 사건이 발생했음을 알릴때 사용하면 편리

    public delegate void TestDelegate1();    // 델리게이트 타입을 하나 생성(델리게이트의 이름은 TestDelegate1)
                                            // (이 델리게이트는 파라메터가 없고 리턴값도 없는 함수만 저장할 수 있다.)

    TestDelegate1 aaa;   // TestDelegate타입으로 함수를 저장할 수 있는 aaa라는 변수를 만듬)
#if UNITY_EDITOR
    void TestRun1()
    {
        Debug.Log("TestRun1");
    }

    void TestRun2()
    {
        Debug.Log("TestRun2");
    }

    void TestRun3()
    {
        Debug.Log("TestRun3");
    }

    public delegate int TestDelegate2(int a, float b);  // 이 델리게이트는 파라메터가 2개(int, float)고 리턴으로 int를 한다.
    TestDelegate2 bbb;
    int TestRun4(int a, float b)
    {
        return a + (int)b;
    }


    private void Start()
    {
        aaa = TestRun1;  // 이전에 등록된 함수는 모두 무시하고 TestRun만 추가
        aaa += TestRun2; // 이전에 등록된 함수 뒤에 TestRun 추가
        aaa = TestRun3 + aaa;    // aaa 맨 앞에 TestRun 추가

        bbb = TestRun4;
        
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        aaa();  // aaa 델리게이트에 등록된 모든 함수 실행하기
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        aaa = null;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        //if(aaa != null)
        //{
        //    aaa();
        //}

        aaa?.Invoke();  // aaa가 null이 아닐때만 실행

        // nullable 타입(null값을 가질 수 있는 타입)
        //int? i;
        //i = null;
        //i = 10;

    }

    // Action : 리턴값이 없는 함수를 저장할 수 있는 델리게이트
    Action ccc; // 파라메터없고 리턴값 없는 함수를 저장할 수 있는 델리게이트
    Action<int> ddd;    // 파라메터로 int 하나 사용하고 리턴값 없는 함수를 저장할 수 있는 델리게이트
    Action<int,int> eee;    // 파라메터로 int 하나 사용하고 리턴값 없는 함수를 저장할 수 있는 델리게이트

    Func<int> f;    // 리턴 타입이 int인 함수를 저장할 수 있는 델리게이트
    Func<int,float> g;    // 파라메터가 int 하나고, 리턴 타입이 float인 함수를 저장할 수 있는 델리게이트

    UnityEvent u1;

    void Test_Unity_Del()
    {

    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        u1.AddListener(Test_Unity_Del);        
    }
#endif
}


// 실습
// 1. 적이 플레이어에게 점수를 주는 방식을 델리게이트로 처리하도록 변경하기
//      사건이 일어나는 곳(델리게이트 실행), 실제로 작용이 일어나는 곳(델리게이트에 함수를 등록)
// 2. 점수가 표시될 때 목표점수까지 순차적으로 증가하는 것처럼 보이게 만들기(UI)