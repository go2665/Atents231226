using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_05_ShipDeployment : Test_04_ShipMovement
{
    // 목표
    // 1. 1~5번 단축키로 함선 선택(선택된 배는 활성화해서 보인다)
    // 2. 보드에 좌클릭을 하면 선택된 함선이 배치
    // 3. 함선이 있는 부분을 우클릭하면 배가 배치 해제가 된다.(해제된 배는 비활성화해서 안보인다.)

    // 4. 머티리얼 변경하기(선택되었을 때 배치가능한 지역이면 반투명녹색, 불가능한 지역이면 반투명 빨간색, 배치 완료되었으면 Normal)
    // 5. 선택이 변경되었을 때 배치 안된 함선은 다시 안보이게 만들기

    /// <summary>
    /// 모든 배
    /// </summary>
    protected Ship[] testShips;

    /// <summary>
    /// 현재 배치하기 위해 선택 중인 배를 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    Ship TargetShip
    {
        get => ship;
        set
        {
            if(ship != null && !ship.IsDeployed)    
            {
                ship.gameObject.SetActive(false);   // 이전 배는 안보이게 만들기(이전 배가 있고 배치가 되지 않았을 때만)
            }

            ship = value;

            if (ship != null && !ship.IsDeployed)    // 새로 배가 설정되면
            {
                ship.SetMaterialType(false);        // 머티리얼 배치 모드로 바꾸기
                ship.transform.position = board.GridToWorld(board.GetMouseGridPosition());  // 마우스 위치로 배 옮기고
                OnShipMovement();                   // 배치 가능한지 표시하기
                ship.gameObject.SetActive(true);    // 배 보이게 만들기
            }
        }
    }

    protected virtual void Start()
    {
        testShips = new Ship[ShipManager.Instance.ShipTypeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Instance.MakeShip(ShipType.Carrier, transform);
        testShips[(int)ShipType.BattleShip - 1] = ShipManager.Instance.MakeShip(ShipType.BattleShip, transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Instance.MakeShip(ShipType.Destroyer, transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Instance.MakeShip(ShipType.Submarine, transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Instance.MakeShip(ShipType.PatrolBoat, transform);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        TargetShip = testShips[(int)ShipType.Carrier - 1];
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        TargetShip = testShips[(int)ShipType.BattleShip - 1];
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        TargetShip = testShips[(int)ShipType.Destroyer - 1];
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        TargetShip = testShips[(int)ShipType.Submarine - 1];
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        TargetShip = testShips[(int)ShipType.PatrolBoat - 1];
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        // 배치용으로 선택된 배가 있으면, 배를 배치 시도(가능하면 배치까지 처리)
        if(TargetShip != null && board.ShipDeployment(TargetShip, board.GetMouseGridPosition()) )
        {
            Debug.Log($"배치 성공 : {TargetShip.gameObject.name}");
            TargetShip = null;
        }
        else
        {
            //Debug.Log("배치 실패");
        }
    }

    protected override void OnTestRClick(InputAction.CallbackContext context)
    {
        Vector2Int grid = board.GetMouseGridPosition();
        ShipType shipType = board.GetShipTypeOnBoard(grid);
        if(shipType != ShipType.None)       // 우클릭 된 지점에 배가 있으면
        {
            Ship ship = testShips[(int)shipType - 1];
            board.UndoShipDeployment(ship); // 배치 취소
        }
    }

    /// <summary>
    /// 배에 움직임이 있었을 때 그 상태로 배치 가능한지 아닌지 여부를 판단해서 머티리얼의 색상을 변경하는 함수
    /// </summary>
    protected override void OnShipMovement()
    {
        bool isSuccess = board.IsShipDeploymentAvailable(TargetShip, TargetShip.transform.position);    // 배 배치 가능한지 확인
        ShipManager.Instance.SetDeployModeColor(isSuccess); // 결과에 따라 색상 설정
    }

}
