using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Instantiate : TestBase
{
    public GameObject prefab;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        new GameObject();   // 비어있는 게임 오브젝트 만들기
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Instantiate(prefab);    // 프리팹을 이용해서 게임 오브젝트 만들기. 로컬 좌표로 (0,0,0)

        // 로컬 좌표 : 부모를 기준으로 한 좌표(부모가 없으면 월드가 부모)
        // 월드 좌표 : 맵(월드)의 원점(origin)을 기준으로 한 좌표
    }

    
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 함수 오버로딩 : 함수의 파라메터만 다르게 하여 다양한 입력을 받을 수 있게 해주는 것

        Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity); // 게임오브젝트를 (5,0,0)에 만들기, 회전 없음
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Instantiate(prefab, this.transform);
    }
}
