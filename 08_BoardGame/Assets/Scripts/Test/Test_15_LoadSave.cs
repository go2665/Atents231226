using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_15_LoadSave : TestBase
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if(gameManager.SaveShipDeplyData())
        {
            Debug.Log("저장 성공");
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if(gameManager.LoadShipDeployData())
        {
            Debug.Log("로딩 성공");
        }
        else
        {
            Debug.Log("로딩 실패");
        }
    }
}
