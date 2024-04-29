using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_05_ShipDeployment : Test_04_ShipMovement
{
    // 목표
    // 1. 1~5번 단축키로 함선 선택(선택된 배는 활성화해서 보인다)
    // 2. 보드에 좌클릭을 하면 선택된 함선이 배치
    // 3. 함선이 있는 부분을 우클릭하면 배가 배치 해제가 된다.(해제된 배는 비활성화해서 안보인다.)

    Ship[] testShips;

    Ship TargetShip
    {
        get => ship;
        set
        {
            ship = value;
        }
    }

    private void Start()
    {
        testShips = new Ship[ShipManager.Instance.ShipTypeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Instance.MakeShip(ShipType.Carrier, transform);
        testShips[(int)ShipType.BattleShip - 1] = ShipManager.Instance.MakeShip(ShipType.BattleShip, transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Instance.MakeShip(ShipType.Destroyer, transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Instance.MakeShip(ShipType.Submarine, transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Instance.MakeShip(ShipType.PatrolBoat, transform);
    }
}
