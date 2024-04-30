using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_06_AutoShipDeployment : Test_05_ShipDeployment
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    protected override void Start()
    {
        base.Start();

        reset.onClick.AddListener(ClearBoard);
        random.onClick.AddListener(AutoShipDeployment);
        resetAndRandom.onClick.AddListener(ClearBoard);
        resetAndRandom.onClick.AddListener(AutoShipDeployment);
    }

    void AutoShipDeployment()
    {
        Debug.Log("함선 자동 배치 실행");
    }

    void ClearBoard()
    {
        Debug.Log("보드 초기화");
        board.ResetBoard(testShips);
    }
}

// 실습
// 1. reset버튼을 누르면 배치되어 있는 모든 배가 배치 해제된다.
// 2. random버튼을 누르면 아직 배치되지 않은 모든 배가 자동으로 배치된다.
//  2.1. 랜덤 배치되는 위치는 우선순위가 높은 위치와 낮은 위치가 있다.
//  2.2. 우선 순위가 낮은 위치(보드 가장자리, 다른 배의 주변 위치)