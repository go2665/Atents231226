using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : PlayerBase
{
    /// <summary>
    /// 지금 배치 하려는 함선
    /// </summary>
    Ship selectedShip;

    /// <summary>
    /// 현재 배치하기 위해 선택 중인 배를 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    protected Ship SelectedShip
    {
        get => selectedShip;
        set
        {
            if (selectedShip != null && !selectedShip.IsDeployed)
            {
                selectedShip.gameObject.SetActive(false);           // 이전 배는 안보이게 만들기(이전 배가 있고 배치가 되지 않았을 때만)
            }

            selectedShip = value;

            if (selectedShip != null && !selectedShip.IsDeployed)   // 새로 배가 설정되면
            {
                selectedShip.SetMaterialType(false);                // 머티리얼 배치 모드로 바꾸기
                selectedShip.transform.position = board.GridToWorld(board.GetMouseGridPosition());  // 마우스 위치로 배 옮기고
                selectedShip.Rotate(false);                         // 서쪽을 바라보게 만들기
                SetSelectedShipColor();                             // 배치 가능한지 표시하기
                selectedShip.gameObject.SetActive(true);            // 배 보이게 만들기
            }
        }
    }

    /// <summary>
    /// 모든 함선이 배치되었는지 확인하는 프로퍼티(true면 모든 함선이 배치되었다)
    /// </summary>
    public bool IsAllDeployed
    {
        get
        {
            bool result = true;
            foreach (var ship in Ships)
            {
                if(!ship.IsDeployed)
                {
                    result = false; // 함선이 하나라도 배치되지 않았으면 false로 끝내기
                    break;
                }
            }
            return result;
        }
    }

    protected override void Start()
    {
        base.Start();

        // 상대방 설정
        opponent = gameManager.EnemyPlayer;

        // 인풋 컨트롤러에 함수 등록
        gameManager.InputController.onMouseClick += OnMouseClick;
        gameManager.InputController.onMouseMove += OnMouseMove;
        gameManager.InputController.onMouseWheel += OnMouseWheel;
    }

    // 함선 배치 및 해제용 함수 ---------------------------------------------------------------------

    /// <summary>
    /// 함선을 배치하기 위해 선택하는 함수
    /// </summary>
    /// <param name="shipType">선택할 함선의 종류</param>
    public void SelectShipToDeploy(ShipType shipType)
    {
        SelectedShip = Ships[(int)shipType - 1];
    }

    /// <summary>
    /// 특정 종류의 함선을 배치 해제하는 함수
    /// </summary>
    /// <param name="shipType">배치 취소할 함수</param>
    public void UndoShipDeploy(ShipType shipType)
    {
        Board.UndoShipDeployment(ships[(int)shipType - 1]); // 보드를 이용해서 한번에 처리
    }

    /// <summary>
    /// 배에 움직임이 있었을 때 그 상태로 배치 가능한지 아닌지 여부를 판단해서 머티리얼의 색상을 변경하는 함수
    /// </summary>
    private void SetSelectedShipColor()
    {
        bool isSuccess = Board.IsShipDeploymentAvailable(SelectedShip, SelectedShip.transform.position);    // 배 배치 가능한지 확인
        shipManager.SetDeployModeColor(isSuccess); // 결과에 따라 색상 설정
    }


    // 입력 처리용 함수 --------------------------------------------------------------------------------
        
    /// <summary>
    /// 마우스 클릭 입력이 있을 때 실행될 함수
    /// </summary>
    /// <param name="position">마우스 포인터의 스크린 좌표</param>
    private void OnMouseClick(Vector2 position)
    {
        if( gameManager.GameState == GameState.ShipDeployment )
        {
            // 게임이 함선 배치 모드일 때의 처리
            if(SelectedShip != null)    
            {
                // 배치할 함선이 있으면
                if (board.ShipDeployment(SelectedShip, board.GetMouseGridPosition()))    // 함선 배치 시도
                {
                    //Debug.Log($"배치 성공 : {SelectedShip.gameObject.name}");
                    SelectedShip = null;    // 성공했으면 배치할 함선을 null로 비우기
                }
                else
                {
                    //Debug.Log($"배치 불가능한 지역");
                }
            }
            else
            {
                // 배치할 함선이 없으면
                Vector2Int grid = board.GetMouseGridPosition();
                ShipType shipType = board.GetShipTypeOnBoard(grid); // 클릭 된 지점에 함선이 있는지 확인
                if (shipType != ShipType.None)      
                {                    
                    // 함선이 있으면
                    Ship ship = GetShip(shipType);
                    board.UndoShipDeployment(ship); // 배치 취소
                }
                else
                {
                    //Debug.Log($"함선이 없음");
                }
            }
        }
        else if( gameManager.GameState == GameState.Battle )
        {
            // 게임이 전투 상태일 때 처리
            Vector2Int grid = opponent.Board.GetMouseGridPosition();
            Attack(grid);
        }
    }

    /// <summary>
    /// 마우스 커서가 움직일 때 실행되는 함수
    /// </summary>
    /// <param name="_">사용안함. 마우스 커서의 스크린 좌표</param>
    private void OnMouseMove(Vector2 _)
    {
        if (gameManager.GameState == GameState.ShipDeployment)
        {
            if (SelectedShip != null)
            {
                // 배치하려는 함선이 있으면 
                //Debug.Log(context.ReadValue<Vector2>());
                Vector2Int grid = board.GetMouseGridPosition();
                Vector3 world = board.GridToWorld(grid);
                SelectedShip.transform.position = world;    // 그리드 가운데 위치로 옮기기

                SetSelectedShipColor(); // 배치 가능 여부에 따라 색상 설정
            }
        }
    }

    /// <summary>
    /// 마우스 휠 입력이 있을 때 실행되는 함수
    /// </summary>
    /// <param name="delta">휠이 움직인 정도</param>
    private void OnMouseWheel(float delta)
    {
        if (gameManager.GameState == GameState.ShipDeployment)
        {
            // 함선 배치 모드이고
            if(SelectedShip != null)    // 배치중인 함선이 있으면
            {
                //Debug.Log(delta);
                SelectedShip.Rotate(delta < 0); // 방향에 맞게 회전시키기
                SetSelectedShipColor();         // 배치 가능 여부에 따라 색상 설정
            }
        }
    }

#if UNITY_EDITOR
    public void Test_BindInputFuncs()
    {
        gameManager.InputController.onMouseClick += OnMouseClick;
        gameManager.InputController.onMouseMove += OnMouseMove;
        gameManager.InputController.onMouseWheel += OnMouseWheel;
    }
#endif
}
