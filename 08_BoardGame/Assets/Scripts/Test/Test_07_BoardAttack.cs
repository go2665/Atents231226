using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_07_BoardAttack : Test_06_AutoShipDeployment
{
    protected override void Start()
    {
        base.Start();
        AutoShipDeployment();
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        base.OnTestLClick(context);

        // 배치할 배가 선택중이 아니면 공격
        if(TargetShip == null)
        {
            Vector2Int attackPos = board.GetMouseGridPosition();
            Debug.Log($"{attackPos} 공격");
            board.OnAttacked(attackPos);
        }
    }
}
