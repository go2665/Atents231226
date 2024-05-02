using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    protected override void Start()
    {
        base.Start();

        opponent = gameManager.EnemyPlayer;
    }

    // 함선 배치 및 해제용 함수 ---------------------------------------------------------------------
    
    /// <summary>
    /// 특정 종류의 함선을 배치 해제하는 함수
    /// </summary>
    /// <param name="shipType">배치 취소할 함수</param>
    public void UndoShipDeploy(ShipType shipType)
    {
        Board.UndoShipDeployment(ships[(int)shipType - 1]); // 보드를 이용해서 한번에 처리
    }

}
