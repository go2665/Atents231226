using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_11_UserShipDeployment : TestBase
{
    public DeploymentToggle toggle;

    private void Start()
    {
        GameManager.Instance.GameState = GameState.ShipDeployment;
        GameManager.Instance.UserPlayer.Test_BindInputFuncs();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(0);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(1);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        toggle.Test_StateChange(2);
    }
}

// 실습
// 1. UserPlayer에 함선 배치 기능 코드 추가하기(Test_05_ShipDeployment 참조)
// 2. 토글 버튼이 Select 상태가 되면 함선을 배치할 수 있어야 한다.
// 3. 배치가 완료되면 토글 버튼이 Deployed 상태로 변경되어야 한다.