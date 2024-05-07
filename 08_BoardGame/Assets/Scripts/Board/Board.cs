using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 보드 크기
    /// </summary>
    public const int BoardSize = 10;

    /// <summary>
    /// 보드에 배치되어 있는 배의 정보(빈곳은 None) 
    /// </summary>
    ShipType[] shipInfo;

    /// <summary>
    /// 공격을 받은 위치면 true, 공격을 받은 적이 없는 위치면 false
    /// </summary>
    bool[] isAttacked;

    /// <summary>
    /// 함선의 종류별로 공격을 당했을 때 실행될 델리게이트를 가지는 딕셔너리
    /// </summary>
    public Dictionary<ShipType, Action> onShipAttacked;

    BombSetter bombSetter;

    private void Awake()
    {
        shipInfo = new ShipType[BoardSize * BoardSize]; // 기본적으로 None으로 세팅
        isAttacked = new bool[BoardSize * BoardSize];   // 기본적으로 모두 false로 세팅

        bombSetter = GetComponentInChildren<BombSetter>();

        onShipAttacked = new Dictionary<ShipType, Action>();
        onShipAttacked[ShipType.None] = null;       // None은 사용하지 않음. 없는 키 참조하는 것 방지용.
        onShipAttacked[ShipType.Carrier] = null;
        onShipAttacked[ShipType.BattleShip] = null;
        onShipAttacked[ShipType.Destroyer] = null;
        onShipAttacked[ShipType.Submarine] = null;
        onShipAttacked[ShipType.PatrolBoat] = null;
    }

    // 함선 배치 관련 함수들 -----------------------------------------------------------------------------------------
    
    /// <summary>
    /// 함선을 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선(위치, 방향, 크기 등의 정보 사용)</param>
    /// <param name="grid">배치될 위치(함선의 머리 위치, 그리드 좌표)</param>
    /// <returns>배치 성공하면 true, 아니면 false</returns>
    public bool ShipDeployment(Ship ship, Vector2Int grid)
    {
        bool result = IsShipDeploymentAvailable(ship, grid, out Vector2Int[] gridPositions);    // 배치 가능한 위치인지 확인
        if(result)
        {
            // 배치가 가능할 때만 처리
            foreach(var pos in gridPositions)
            {
                shipInfo[GridToIndex(pos).Value] = ship.Type;   // shipInfo에 함선 기록
            }

            Vector3 world = GridToWorld(grid);
            ship.transform.position = world;    // 함선 위치를 배치할 위치로 옮기고
            ship.Deploy(gridPositions);         // 함선 개별 배치 처리 실행
        }

        return result;
    }

    /// <summary>
    /// 함선을 배치하는 함수
    /// </summary>
    /// <param name="ship">배치할 함선(위치, 방향, 크기 등의 정보 사용)</param>
    /// <param name="world">배치될 위치(함선의 머리 위치, 월드좌표)</param>
    /// <returns>배치 성공하면 true, 아니면 false</returns>
    public bool ShipDeployment(Ship ship, Vector3 world)
    {
        return ShipDeployment(ship, WorldToGrid(world));
    }

    /// <summary>
    /// 함선이 특정 위치에 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배치 위치(함선의 머리 위치, 그리드 좌표)</param>
    /// <param name="resultPositions">배치가 가능할 때 배치될 위치들(그리드 좌표)</param>
    /// <returns>true면 배치가능, false면 배치 불가능</returns>
    public bool IsShipDeploymentAvailable(Ship ship, Vector2Int grid, out Vector2Int[] resultPositions)
    {
        Vector2Int dir = Vector2Int.zero;   // 그리드 위치 계산을 위한 방향 벡터
        switch (ship.Direction)             // 바라보는 방향에 따라 방향 벡터 값 결정
        {
            case ShipDirection.North:
                dir = Vector2Int.up;    // y는 아래로 갈수록 값이 커지니까
                break;
            case ShipDirection.East:
                dir = Vector2Int.left;
                break;
            case ShipDirection.South:
                dir = Vector2Int.down;
                break;
            case ShipDirection.West:
                dir = Vector2Int.right;
                break;
            default:
                break;
        }

        resultPositions = new Vector2Int[ship.Size];    // 확인할 위치 만들기(or 배치 가능한 경우의 배치 위치)
        for(int i=0;i<ship.Size;i++)
        {
            resultPositions[i] = grid + dir * i;
        }

        bool result = true;     // 모든 위치가 배치가능하면 true
        foreach (var pos in resultPositions)                    // 확인할 위치들을 하나씩 확인하기
        {
            if(!IsInBoard(pos) || IsShipDeployedPosition(pos))  // 하나라도 보드밖이거나 배가 배치되어있으면
            {
                result = false; // 배치 불가능
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 함선이 특정 위치에 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="grid">확인할 배치 위치(함선의 머리 위치, 그리드 좌표)</param>
    /// <returns>true면 배치가능, false면 배치 불가능</returns>
    public bool IsShipDeploymentAvailable(Ship ship, Vector2Int grid)
    {
        return IsShipDeploymentAvailable(ship, grid, out _);
    }

    /// <summary>
    /// 함선이 특정 위치에 배치될 수 있는지 확인하는 함수
    /// </summary>
    /// <param name="ship">확인할 배</param>
    /// <param name="world">확인할 배치 위치(함선의 머리 위치, 월드 좌표)</param>
    /// <returns>true면 배치가능, false면 배치 불가능</returns>
    public bool IsShipDeploymentAvailable(Ship ship, Vector3 world)
    {
        return IsShipDeploymentAvailable(ship, WorldToGrid(world), out _);
    }

    /// <summary>
    /// 보드의 특정 위치에 있는 배의 종류를 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치(그리드 좌표)</param>
    /// <returns>배가 없거나 보드 밖이면 ShipType.None, 배가 있으면 그 배의 종류</returns>
    public ShipType GetShipTypeOnBoard(Vector2Int grid)
    {
        ShipType result = ShipType.None;

        int? index = GridToIndex(grid); // 보드 안이고 그리드 값이 보드 크기 안일 때 값이 나옴(아니면 null)
        if(index != null)
        {
            result = shipInfo[GridToIndex(grid).Value]; // 제대로 된 값일 때 배의 정보 리턴
        }

        return result;
    }

    /// <summary>
    /// 보드의 특정 위치에 있는 배의 종류를 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치(월드좌표)</param>
    /// <returns>배가 없거나 보드 밖이면 ShipType.None, 배가 있으면 그 배의 종류</returns>
    public ShipType GetShipTypeOnBoard(Vector3 world)
    {
        return GetShipTypeOnBoard(WorldToGrid(world));
    }

    /// <summary>
    /// 함선 배치 해제 하는 함수
    /// </summary>
    /// <param name="ship">배치 해제할 함선</param>
    public void UndoShipDeployment(Ship ship)
    {
        if(ship.IsDeployed) // 배치된 배만 배치 해제할 수 있다.
        {
            foreach(var pos in ship.Positions)
            {
                shipInfo[GridToIndex(pos).Value] = ShipType.None;   // 기록된 위치들을 초기화
            }
            ship.UnDeploy();                    // 함선 개별 배치 해제 처리
            ship.gameObject.SetActive(false);   // 함선 안보이게 만들기
        }
    }

    /// <summary>
    /// 보드 초기화용 함수
    /// </summary>
    /// <param name="ships">배치 해제할 배들</param>
    public void ResetBoard(Ship[] ships)
    {
        // ships 전부 배치 해제
        foreach (var ship in ships)
        {
            UndoShipDeployment(ship);
        }

        // 공격 받은 위치 초기화
        for(int i = 0;i<isAttacked.Length;i++)
        {
            isAttacked[i] = false;
        }

        // 폭탄 마크도 전부 리셋
        bombSetter.ResetBombMarks();
    }


    // 공격 관련 함수들 ----------------------------------------------------------------------------------------------
    

    /// <summary>
    /// 보드가 상대방에 의해 공격을 당했을 때 실행되는 함수
    /// </summary>
    /// <param name="grid">공격 받은 위치(그리드좌표), 이 위치는 반드시 보드 안쪽이고 공격 가능한 위치이다.</param>
    /// <returns>true면 공격이 함선에 명중했다, false면 공격이 함선에 맞지 않았다.</returns>
    public bool OnAttacked(Vector2Int grid)
    {
        bool result = false;

        int index = GridToIndex(grid).Value;
        isAttacked[index] = true;               // 해당 위치에 공격 했다고 표시

        ShipType target = shipInfo[index];      // 공격 받은 위치에 있는 함선 종류 확인
        if( target != ShipType.None )
        {
            result = true;
            onShipAttacked[target]?.Invoke();   // 함선 종류에 맞게 명중했다고 신호 보내기
        }

        bombSetter.SetBombMark(GridToWorld(grid), result);  // 붐바크로 표시하기


        return result;
    }

    public bool IsAttackable(int index)
    {
        return !isAttacked[index];
    }

    public bool IsAttackable(Vector2Int grid)
    {
        bool result = false;
        int? index = GridToIndex(grid);
        if(index.HasValue)       
            result = IsAttackable(index.Value);

        return result;
    }


    /// <summary>
    /// 특정 위치가 공격이 성공한 위치인지를 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치</param>
    /// <returns>true면 공격이 성공한 위치, false면 공격이 실패한 위치</returns>
    public bool IsAttackSuccessPosition(Vector2Int grid)
    {
        int? index = GridToIndex(grid);

        // grid가 보드 안쪽이고, 공격 당한 위치고, 배가 있다.
        return index != null && isAttacked[index.Value] && shipInfo[index.Value] != ShipType.None;
    }

    /// <summary>
    /// 특정 위치가 공격이 실패한 위치인지를 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치</param>
    /// <returns>true면 공격이 실패한 위치, false면 공격이 성공한 위치</returns>
    public bool IsAttackFailPosition(Vector2Int grid)
    {
        int? index = GridToIndex(grid);

        // grid가 보드 안쪽이고, 공격 당한 위치고, 배가 없다.
        return index != null && isAttacked[index.Value] && shipInfo[index.Value] == ShipType.None;
    }

    // 좌표 변환용 유틸리티 함수들-------------------------------------------------------------------------------------

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>보드 상의 그리드 위치</returns>
    public Vector2Int IndexToGrid(uint index)
    {
        return new Vector2Int((int)index % BoardSize, (int)index / BoardSize);
    }

    /// <summary>
    /// 인덱스 값을 월드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>계산된 월드 좌표</returns>
    public Vector3 IndexToWorld(uint index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변경하는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <returns>그리드 좌표가 보드 안이면 해당하는 인덱스, 아니면 null</returns>
    public int? GridToIndex(int x, int y)
    {
        int? result = null;
        if( IsInBoard(x,y) )
        {
            result = x + y * BoardSize;
        }
        return result;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변경하는 함수
    /// </summary>
    /// <param name="grid">그리드 값</param>
    /// <returns>그리드 좌표가 보드 안이면 해당하는 인덱스, 아니면 null</returns>
    public int? GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <returns>계산된 월드좌표(그리드의 중심점)</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>계산된 월드좌표(그리드의 중심점)</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="world">월드 좌표</param>
    /// <returns>계산된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 world)
    {
        world.y = transform.position.y; // x,z의 차이만 구하기 위해 y는 보드의 위치와 일치시키기

        Vector3 diff = world - transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    } 

    /// <summary>
    /// 마우스 커서 위치의 그리드 좌표
    /// </summary>
    /// <returns>그리드 좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);        
        return WorldToGrid(world);
    }

    // 확인용 함수들 --------------------------------------------------------------------------------------   

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="world">월드좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector3 world)
    {
        world.y = transform.position.y;
        Vector3 diff = world - transform.position;

        return diff.x >= 0 && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(int x, int y)
    {
        return x > -1 && x < BoardSize && y > -1 && y < BoardSize;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector2Int grid)
    {
        return IsInBoard(grid.x, grid.y);
    }

    /// <summary>
    /// 특정 위치에 배가 배치되어 있는지 확인하는 함수
    /// </summary>
    /// <param name="grid">확인할 위치(그리드좌표)</param>
    /// <returns>true면 배가 있다. false면 없다.</returns>
    bool IsShipDeployedPosition(Vector2Int grid)
    {
        int? index = GridToIndex(grid); 
        bool result;
        if(index.HasValue)
            result = shipInfo[index.Value] != ShipType.None;    // index가 값이 있으면 shipInfo 확인
        else
            result = false;

        return result;
    }

}
