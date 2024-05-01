using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_07_BoardAttack : Test_06_AutoShipDeployment
{
    protected override void Start()
    {
        base.Start();
        GetShip(ShipType.Carrier).onHit += (_) => GameManager.Instance.CameraShake(1);
        GetShip(ShipType.BattleShip).onHit += (_) => GameManager.Instance.CameraShake(1);
        GetShip(ShipType.Destroyer).onHit += (_) => GameManager.Instance.CameraShake(1);
        GetShip(ShipType.Submarine).onHit += (_) => GameManager.Instance.CameraShake(1);
        GetShip(ShipType.PatrolBoat).onHit += (_) => GameManager.Instance.CameraShake(1);

        GetShip(ShipType.Carrier).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.BattleShip).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.Destroyer).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.Submarine).onSink += (_) => GameManager.Instance.CameraShake(3);
        GetShip(ShipType.PatrolBoat).onSink += (_) => GameManager.Instance.CameraShake(3);

        board.onShipAttacked[ShipType.Carrier] += GetShip(ShipType.Carrier).OnHitted;
        board.onShipAttacked[ShipType.BattleShip] += GetShip(ShipType.BattleShip).OnHitted;
        board.onShipAttacked[ShipType.Destroyer] += GetShip(ShipType.Destroyer).OnHitted;
        board.onShipAttacked[ShipType.Submarine] += GetShip(ShipType.Submarine).OnHitted;
        board.onShipAttacked[ShipType.PatrolBoat] += GetShip(ShipType.PatrolBoat).OnHitted;

        AutoShipDeployment();

        //GetShip(ShipType.Carrier).Test_SinkBanner();
        //GetShip(ShipType.BattleShip).Test_SinkBanner();
        //GetShip(ShipType.Destroyer).Test_SinkBanner();
        //GetShip(ShipType.Submarine).Test_SinkBanner();
        //GetShip(ShipType.PatrolBoat).Test_SinkBanner();
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        base.OnTestLClick(context);

        // 배치할 배가 선택중이 아니면 공격
        if(TargetShip == null)
        {
            Vector2Int attackPos = board.GetMouseGridPosition();
            if (board.IsInBoard(attackPos) && board.IsAttackable(attackPos))
            {
                //Debug.Log($"{attackPos} 공격");
                board.OnAttacked(attackPos);
            }
        }
    }

    Ship GetShip(ShipType shipType)
    {
        return testShips[(int)shipType - 1];
    }
}

// 코드 확인
// 침몰된 함선을 확실히 표시하기