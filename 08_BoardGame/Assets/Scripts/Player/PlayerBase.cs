using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// 이 플레이어가 가지는 보드
    /// </summary>
    protected Board board;
    public Board Board => board;

    /// <summary>
    /// 일반 공격 후보 지역들의 인덱스들
    /// </summary>
    List<uint> normalAttackIndices;

    /// <summary>
    /// 우선 순위가 높은 공격 후보지역들의 인덱스들
    /// </summary>
    List<uint> criticalAttackIndices;

    /// <summary>
    /// 마지막으로 공격이 성공한 그리드 좌표. NOT_SUCCESS면 이전 공격은 실패한 것
    /// </summary>
    Vector2Int lastSuccessAttackPosition;

    /// <summary>
    /// 이전 공격이 성공하지 않았다고 표시하는 읽기 전용 변수
    /// </summary>
    readonly Vector2Int NOT_SUCCESS = -Vector2Int.one;

    /// <summary>
    /// 이웃 위치 확인용
    /// </summary>
    readonly Vector2Int[] neighbors = { new(-1, 0), new(1, 0), new(0, 1), new(0, -1) };

    /// <summary>
    /// 이번 공격으로 상대방의 함선이 침몰했는지 알려주는 변수(true면 침몰했다, false면 침몰하지 않았다)
    /// </summary>
    bool opponentShipDestroyed = false;

    /// <summary>
    /// 공격 후보지역 표시용 프리팹
    /// </summary>
    public GameObject criticalMarkPrefab;

    /// <summary>
    /// 공격 후보지역들이 생성후 붙을 부모 트랜스폼
    /// </summary>
    Transform criticalMarkParent;

    /// <summary>
    /// 생성한 공격 후보지역들
    /// </summary>
    Dictionary<uint, GameObject> criticalMarks;

    /// <summary>
    /// 이 플레이어가 가지는 함선들
    /// </summary>
    protected Ship[] ships;
    public Ship[] Ships => ships;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    /// <summary>
    /// 행동여부를 표시하는 변수(true면 행동완료, false면 미행동)
    /// </summary>
    bool isActionDone = false;

    /// <summary>
    /// 이 플레이어의 행동이 완료되었음을 알리는 델리게이트
    /// </summary>
    public Action onActionEnd;

    /// <summary>
    /// 이 플레이어가 패배했음을 알리는 델리게이트
    /// </summary>
    public Action<PlayerBase> onDefeat;

    /// <summary>
    /// 아직 침몰하지 않은 함선의 수
    /// </summary>
    int remainShipCount;    

    /// <summary>
    /// 함선 매니저
    /// </summary>
    protected ShipManager shipManager;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    protected GameManager gameManager;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    protected TurnManager turnManager;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        board = child.GetComponent<Board>();

        criticalMarkParent = transform.GetChild(1);
        criticalMarks = new Dictionary<uint, GameObject>(10);
    }

    protected virtual void Start()
    {
        shipManager = ShipManager.Instance;
        gameManager = GameManager.Instance;
        turnManager = gameManager.TurnManager;

        Initialize();
    }

    protected void Initialize()
    {
        // 함선 생성
        int count = shipManager.ShipTypeCount;
        ships = new Ship[count];
        for(int i=0; i < count; i++)
        {
            ShipType shipType = (ShipType)i + 1;
            ships[i] = shipManager.MakeShip(shipType, transform);   // 종류별로 함선 생성

            ships[i].onHit += (_) => gameManager.CameraShake(1);    // 명중시와 침몰시 카메라 흔들림 추가
            ships[i].onSink += (_) => gameManager.CameraShake(3);
            ships[i].onSink += OnShipDestroy;

            board.onShipAttacked[shipType] += ships[i].OnHitted;    // 공격 당했을 때 실행할 함수 연결
        }
        remainShipCount = count;    // 함선 개수 초기화

        // 보드 초기화
        Board.ResetBoard(ships);

        // 공격 관련 초기화
        int fullSize = Board.BoardSize * Board.BoardSize;

        // 일반 공격 후보지역 만들기
        uint[] temp = new uint[fullSize];
        for(uint i=0; i < fullSize; i++)
        {
            temp[i] = i;    // 배열 순서대로 채우고
        }
        Util.Shuffle(temp); // 섞은 후
        normalAttackIndices = new List<uint>(temp);  // 리스트로 만들기

        criticalAttackIndices = new List<uint>(10);  // 우선 순위가 높은 공격 후보지역 만들기(비어있음)

        lastSuccessAttackPosition = NOT_SUCCESS;    // 이전 공격이 성공한 적 없다고 초기화

        turnManager.onTurnStart += OnPlayerTurnStart;   // 턴 시작 함수 연결
        turnManager.onTurnEnd += OnPlayerTurnEnd;       // 턴 종료 함수 연결

    }

    // 함선배치 관련 함수 --------------------------------------------------------------------------
    /// <summary>
    /// 아직 배치되지 않는 배를 모두 자동으로 배치하는 함수
    /// </summary>
    /// <param name="isShipShow">true면 배치 후 함선 표시, false면 미표시</param>
    public void AutoShipDeployment(bool isShipShow)
    {
        //Debug.Log("함선 자동 배치 실행");

        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new List<int>(maxCapacity);
        List<int> low = new List<int>(maxCapacity);

        // 가장자리 부분은 low에 넣고 남은 부분은 high에 넣기
        for (int i = 0; i < maxCapacity; i++)
        {
            if ((i % Board.BoardSize == 0)                          // 0, 10, 20, 30, 40, 50, 60, 70, 80, 90
                || (i % Board.BoardSize == (Board.BoardSize - 1))   // 9, 19, 29, 39, 49, 59, 69, 79, 89, 99
                || (i > 0 && i < Board.BoardSize - 1)               // 1, 2, 3, 4, 5, 6, 7, 8
                || (Board.BoardSize * (Board.BoardSize - 1) < i && i < Board.BoardSize * Board.BoardSize - 1))  // 91 ~ 98
            {
                low.Add(i);
            }
            else
            {
                high.Add(i);
            }
        }

        // 이미 배치된 배에 대한 처리
        foreach (var ship in Ships)
        {
            if (ship.IsDeployed)
            {
                int[] shipIndice = new int[ship.Size];
                for (int i = 0; i < ship.Size; i++)
                {
                    shipIndice[i] = board.GridToIndex(ship.Positions[i]).Value; // 배가 배치된 부분의 인덱스 구하기
                }

                foreach (var index in shipIndice)
                {
                    high.Remove(index);     // 이미 배가 배치된 위치는 high, low 모두에서 제거
                    low.Remove(index);
                }

                List<int> toLow = GetShipAroundPositions(ship);     // ship의 주변 위치 구하기
                foreach (var index in toLow)
                {
                    high.Remove(index);
                    low.Add(index);
                }
            }
        }

        // high와 low 내부의 순서 섞기
        int[] temp = high.ToArray();
        Util.Shuffle(temp);
        high = new(temp);
        temp = low.ToArray();
        Util.Shuffle(temp);
        low = new(temp);

        // 함선 배치 시작
        foreach (var ship in Ships)
        {
            if (!ship.IsDeployed)        // 배치되어 있는 경우만 처리
            {
                ship.RandomRotate();    // 함선의 방향을 랜덤으로 결정

                bool fail = true;           // 배치 가능 여부
                int count = 0;              // 배치 시도 회수
                const int maxHighCount = 10;// 최대 시도 회수
                Vector2Int grid;            // 함선 머리 위치(그리드)
                Vector2Int[] shipPositions; // 한선이 배치 가능할 때의 배치되는 위치들

                // high에서 위치 고르기
                do
                {
                    int head = high[0];     // high에서 첫번째 아이템 가져오기
                    high.RemoveAt(0);

                    grid = board.IndexToGrid((uint)head);       // 머리 인덱스를 그리드 좌표로 바꾸기
                    fail = !board.IsShipDeploymentAvailable(ship, grid, out shipPositions); // 함선이 배치 가능한지 확인
                    if (fail)
                    {
                        // 함선 머리 부분이 배치불가능하면 high에 되돌리기
                        high.Add(head);
                    }
                    else
                    {
                        // 함선의 머리 부분이 배치 가능하면 남은 부분도 high에 있는지 확인
                        for (int i = 1; i < shipPositions.Length; i++)
                        {
                            int body = board.GridToIndex(shipPositions[i]).Value;
                            if (!high.Contains(body))
                            {
                                // high에 나머지 부분이 없으면 high에 되돌리고 실패 처리
                                high.Add(head);
                                fail = true;
                                break;
                            }
                        }
                    }
                    count++;    // 시도 회수 증가

                    // 실패 했고, 반복회수가 10번 미만이고, high에 아직 아직 인덱스가 남아있으면 반복
                } while (fail && count < maxHighCount && high.Count > 0);

                // low에서 위치 고르기
                count = 0;
                while (fail && count < 1000)
                {
                    int head = low[0];      // low의 첫번째 아이템 꺼내기
                    low.RemoveAt(0);
                    grid = board.IndexToGrid((uint)head);       // 함선 머리 부분의 그리드 좌표 구하기
                    fail = !board.IsShipDeploymentAvailable(ship, grid, out shipPositions); // 배치 가능한지 확인
                    if (fail)
                    {
                        low.Add(head);      // 배치가 불가능하면 low에 되돌리기
                    }
                    count++;    // 시도회수 증가
                }

                // high와 low 모두에서 실패한 경우
                if (fail)
                {
                    Debug.LogWarning("함선 자동배치 실패!");
                    return;
                }

                // 실제 배치
                board.ShipDeployment(ship, grid);
                ship.gameObject.SetActive(isShipShow);

                // 배치된 위치를 high와 low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(board.GridToIndex(pos).Value);
                }
                foreach (var index in tempList)
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                // 배치된 함선 주변 위치를 low로 보내기
                List<int> toLow = GetShipAroundPositions(ship);
                foreach (var index in toLow)
                {
                    if (high.Contains(index))    // high에 있으면 
                    {
                        low.Add(index);         // low로 보내기
                        high.Remove(index);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 함선의 주변 위치들의 인덱스를 구하는 함수
    /// </summary>
    /// <param name="ship">주변 위치를 구할 배</param>
    /// <returns>배의 주변 위치들</returns>
    List<int> GetShipAroundPositions(Ship ship)
    {
        List<int> result = new List<int>(ship.Size * 2 + 6);    // 함선 옆면 * 2, 머리쪽3, 꼬리쪽3
        int? index = null;

        if (ship.Direction == ShipDirection.North || ship.Direction == ShipDirection.South)
        {
            // 함선이 세로로 있는 경우

            // 함선의 옆면 두 줄 넣기
            foreach (var pos in ship.Positions)
            {
                index = board.GridToIndex(pos + Vector2Int.left);   // 보드 안쪽일 경우만 추가하기
                if (index.HasValue) result.Add(index.Value);

                index = board.GridToIndex(pos + Vector2Int.right);
                if (index.HasValue) result.Add(index.Value);
            }

            // 함선의 머리 위쪽, 꼬리 아래쪽
            Vector2Int head;
            Vector2Int tail;

            if (ship.Direction == ShipDirection.North)
            {
                head = ship.Positions[0] + Vector2Int.down;
                tail = ship.Positions[^1] + Vector2Int.up;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.up;
                tail = ship.Positions[^1] + Vector2Int.down;
            }

            index = board.GridToIndex(head);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.left);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.right);
            if (index.HasValue) result.Add(index.Value);

            index = board.GridToIndex(tail);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.left);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.right);
            if (index.HasValue) result.Add(index.Value);
        }
        else
        {
            // 함선이 가로로 있는 경우

            // 함선의 옆면 두 줄 넣기
            foreach (var pos in ship.Positions)
            {
                index = board.GridToIndex(pos + Vector2Int.up);   // 보드 안쪽일 경우만 추가하기
                if (index.HasValue) result.Add(index.Value);

                index = board.GridToIndex(pos + Vector2Int.down);
                if (index.HasValue) result.Add(index.Value);
            }

            // 함선의 머리 위쪽, 꼬리 아래쪽
            Vector2Int head;
            Vector2Int tail;

            if (ship.Direction == ShipDirection.East)
            {
                head = ship.Positions[0] + Vector2Int.right;
                tail = ship.Positions[^1] + Vector2Int.left;
            }
            else
            {
                head = ship.Positions[0] + Vector2Int.left;
                tail = ship.Positions[^1] + Vector2Int.right;
            }

            index = board.GridToIndex(head);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.up);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(head + Vector2Int.down);
            if (index.HasValue) result.Add(index.Value);

            index = board.GridToIndex(tail);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.up);
            if (index.HasValue) result.Add(index.Value);
            index = board.GridToIndex(tail + Vector2Int.down);
            if (index.HasValue) result.Add(index.Value);
        }

        return result;
    }

    /// <summary>
    /// 모든 함선의 배치를 취소하는 함수
    /// </summary>
    public void UndoAllShipDeployment()
    {
        Board.ResetBoard(Ships);
    }

    // 공격 관련 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 적을 공격하는 함수
    /// </summary>
    /// <param name="attackGrid">공격할 위치(그리드 좌표)</param>
    public void Attack(Vector2Int attackGrid)
    {
        Board opponentBoard = opponent.Board;
        if (!isActionDone && opponentBoard.IsInBoard(attackGrid) && opponentBoard.IsAttackable(attackGrid))
        {
            //Debug.Log($"{attackPos} 공격");
            bool result = opponentBoard.OnAttacked(attackGrid);
            if(result)
            {
                if(opponentShipDestroyed)
                {
                    // 지금 공격으로 적의 함선이 침몰한 경우
                    RemoveAllCriticalPositions();   // 우선 순위가 높은 후보지역 모두 제거
                    opponentShipDestroyed = false;  // 확인 되었으니 false로 리셋
                }
                else
                {
                    // 지금 공격으로 적의 함선이 침몰하지 않은 경우

                    if(lastSuccessAttackPosition != NOT_SUCCESS)
                    {
                        // 연속으로 공격이 성공했다. => 한줄로 공격이 성공했다.
                        AddCriticalFromTwoPoint(attackGrid, lastSuccessAttackPosition);
                    }
                    else
                    {
                        // 처음 성공한 공격
                        AddCriticalFromNeighbors(attackGrid);
                    }

                    lastSuccessAttackPosition = attackGrid;
                }
            }
            else
            {
                // 성공->실패->성공 순서였을 때 두번 째 성공에서 주변 모두를 추가하는 문제 수정용
                //lastSuccessAttackPosition = NOT_SUCCESS;  
            }

            uint attackIndex = (uint)board.GridToIndex(attackGrid).Value;
            RemoveCriticalPosition(attackIndex);
            normalAttackIndices.Remove(attackIndex);

            isActionDone = true;
            onActionEnd?.Invoke();
        }
    }

    /// <summary>
    /// 적을 공격하는 함수
    /// </summary>
    /// <param name="world">공격할 위치(월드 좌표)</param>
    public void Attack(Vector3 world)
    {
        Attack(opponent.Board.WorldToGrid(world));
    }

    /// <summary>
    /// 적을 공격하는 함수
    /// </summary>
    /// <param name="index">공격할 위치(인덱스)</param>
    public void Attack(uint index)
    {
        Attack(opponent.Board.IndexToGrid(index));
    }

    /// <summary>
    /// 자동으로 공격하는 함수. Enemy가 공격할 때나 User가 타임 아웃되었을 때 사용하는 목적.
    /// </summary>
    public void AutoAttack()
    {
        // 똑똑하게 다음 목표를 설정하는 방법
        // 1. 무작위로 공격
        // 2. 이전 공격이 성공했을 때 성공한 위치의 위,아래,좌,우 중 한군대를 공격
        // 3. 공격이 한줄로 성공했을 때 

        // 확인할 순서 : 3 -> 2 -> 1

        // 필요한 요소
        // 1. 한줄로 공격이 성공했는지 확인이 가능해야 한다.
        // 2. 이전 공격이 성공했는지 확인이 가능해야 한다.

        // 공격으로 함선이 침몰되면 무조건 1번부터 시작

        uint target;
        if(criticalAttackIndices.Count > 0)      // 우선 순위가 높은 공격 후보 지역이 있는지 확인
        {
            target = criticalAttackIndices[0];   // 있는 것 꺼내기
            criticalAttackIndices.RemoveAt(0);
            normalAttackIndices.Remove(target);  // normal에서도 제거
        }
        else
        {
            target = normalAttackIndices[0];     // 우선 순위가 높은 공격 후보지역이 없으면 normal에서 꺼내기
            normalAttackIndices.RemoveAt(0);
        }

        Attack(target);
    }

    /// <summary>
    /// grid 사방을 우선 순위가 높은 지역으로 설정
    /// </summary>
    /// <param name="grid">기준 위치</param>
    private void AddCriticalFromNeighbors(Vector2Int grid)
    {
        Util.Shuffle(neighbors);
        foreach(var neighbor in neighbors)      // 4방향 추가
        {
            Vector2Int pos = grid + neighbor;
            if( opponent.Board.IsAttackable(pos))
            {
                AddCritical((uint)board.GridToIndex(pos).Value);
            }
        }
    }

    /// <summary>
    /// 현재 성공지점의 양끝을 우선 순위가 높은 후보지역으로 만드는 함수
    /// </summary>
    /// <param name="now">지금 공격 성공한 위치</param>
    /// <param name="last">직전에 공격 성공한 위치</param>
    private void AddCriticalFromTwoPoint(Vector2Int now, Vector2Int last)
    {
        if (IsSuccessLine(last, now, true))
        {
            // 같은 줄에 있는 것이 아니면 제거
            Vector2Int grid;
            List<uint> deleteTarget = new List<uint>(16);
            foreach(var index in criticalAttackIndices)
            {
                grid = Board.IndexToGrid(index);
                if(grid.y != now.y)             // y가 다르면 한줄에 있는 것이 아니다.
                {
                    deleteTarget.Add(index);
                }
            }
            foreach(var index in deleteTarget)
            {
                RemoveCriticalPosition(index);  // 같은 줄에 있는 것이 아니면 삭제
            }

            // 양끝(수평)에 위치를 Critical에 추가
            grid = now;
            for(grid.x = now.x + 1; grid.x<Board.BoardSize; grid.x++ )  // now의 오른쪽 확인해서 추가
            {
                if (!Board.IsInBoard(grid))                             // 보드 밖이면 끝
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))          // 공격 실패한 지역이면 끝
                    break;
                if( opponent.Board.IsAttackable(grid))                  // 공격 가능하면
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);   // 추가하고 끝
                    break;
                }    
            }
            for (grid.x = now.x - 1; grid.x > -1; grid.x--) // now의 왼쪽 확인해서 추가
            {
                if (!Board.IsInBoard(grid))
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))
                    break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
        }
        else if (IsSuccessLine(last, now, false))
        {
            // 같은 줄에 있는 것이 아니면 제거
            Vector2Int grid;
            List<uint> deleteTarget = new List<uint>(16);
            foreach (var index in criticalAttackIndices)
            {
                grid = Board.IndexToGrid(index);
                if (grid.x != now.x)             // x가 다르면 한줄에 있는 것이 아니다.
                {
                    deleteTarget.Add(index);
                }
            }
            foreach (var index in deleteTarget)
            {
                RemoveCriticalPosition(index);  // 같은 줄에 있는 것이 아니면 삭제
            }

            // 양끝(수직)에 위치를 Critical에 추가
            grid = now;
            for (grid.y = now.y + 1; grid.y < Board.BoardSize; grid.y++)  // now의 아래쪽 확인해서 추가
            {
                if (!Board.IsInBoard(grid))
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))
                    break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
            for (grid.y = now.y - 1; grid.y > -1; grid.y--) // now의 위쪽 확인해서 추가
            {
                if (!Board.IsInBoard(grid))
                    break;
                if (opponent.Board.IsAttackFailPosition(grid))
                    break;
                if (opponent.Board.IsAttackable(grid))
                {
                    AddCritical((uint)Board.GridToIndex(grid).Value);
                    break;
                }
            }
        }
        else
        {
            // 같은 줄이 아니다.(= 다른 배다 = 이웃들을 추가한다)
            AddCriticalFromNeighbors(now);
        }
    }

    /// <summary>
    /// 한줄로 공격이 성공했는지(start에서 end까지 모두 공격 성공이었는지)를 체크하는 함수
    /// </summary>
    /// <param name="start">확인 시작점</param>
    /// <param name="end">확인 종료지점</param>
    /// <param name="isHorizontal">true면 가로로 체크, false면 세로로 체크</param>
    /// <returns>같은 라인에 있고 그 사이는 모두 공격 성공이면 true, 다른 라인이거나 하나라도 공격 실패가 있으면 false</returns>
    private bool IsSuccessLine(Vector2Int start, Vector2Int end, bool isHorizontal)
    {
        bool result = true;

        Vector2Int pos = start; // start에서 end까지 순차적으로 위치를 저장할 임시 변수
        int dir = 1;            // start에서 end로 가는 방향( 1 or -1 )

        if(isHorizontal)
        {
            if(start.y == end.y)        // y가 같으면 가로로 된 줄이 맞다.
            {
                if(start.x > end.x)     // start가 end보다 오른쪽에 있다.
                {
                    dir = -1;           // 진행 방향을 반대로 설정(역방향)
                }

                start.x *= dir;         // 역방향일 경우 for문에서 정상적으로 돌리기 위해 뒤집기
                end.x *= dir;
                end.x++;                // end의 x까지 확인하기 위해 1증가

                for (int i = start.x; i<end.x; i++)  // i는 start의 x에서 end의 x까지 증가
                {
                    pos.x = i * dir;
                    if( opponent.Board.IsAttackFailPosition(pos) )  // 공격이 실패한 지점인지 확인
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                // 가로로 된 직선이 아니다
                result = false;
            }
        }
        else
        {
            if (start.x == end.x)       // x가 같으면 세로로 된 줄이 맞다.
            {
                if (start.y > end.y)    // start가 end보다 아래쪽에 있다.
                {
                    dir = -1;           // 진행 방향을 반대로 설정(역방향)
                }

                start.y *= dir;         // 역방향일 경우 for문에서 정상적으로 돌리기 위해 뒤집기
                end.y *= dir;
                end.y++;                // end의 y까지 확인하기 위해 1증가

                for (int i = start.y; i < end.y; i++)  // i는 start의 y에서 end의 y까지 증가
                {
                    pos.y = i * dir;
                    if (opponent.Board.IsAttackFailPosition(pos))  // 공격이 실패한 지점인지 확인
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                // 가로로 된 직선이 아니다
                result = false;
            }
        }
        return result;
    }

    /// <summary>
    /// 우선 순위가 높은 후보지역에 인덱스를 추가하는 함수
    /// </summary>
    /// <param name="index">추가할 인덱스</param>
    private void AddCritical(uint index)
    {
        if(!criticalAttackIndices.Contains(index))  // 없을 때만 추가
        {
            criticalAttackIndices.Insert(0, index); // 항상 앞에 추가(새로 추가되는 위치가 성공 확률이 더 높기 때문)

            // 후보지역 표시
            if(GameManager.Instance.IsTestMode)
            {
                GameObject obj = Instantiate(criticalMarkPrefab, criticalMarkParent);   // 프리팹 생성
                obj.transform.position = opponent.Board.IndexToWorld(index);            // 적 보드 위치에 맞게 위치 수정
                Vector2Int grid = opponent.Board.IndexToGrid(index);        
                obj.name = $"Critical_({grid.x},{grid.y})";     // 이름 알아보기 쉽게 바꾸기
                criticalMarks[index] = obj; // criticalMarks.Add(index, obj);
            }
            
        }
    }

    /// <summary>
    /// 우선 순위가 낮은 후보지역을 제거
    /// </summary>
    /// <param name="index"></param>
    private void RemoveCriticalPosition(uint index)
    {
        if (criticalAttackIndices.Contains(index))  // 공격 후보지역이 있으면
        {
            criticalAttackIndices.Remove(index);    // 공격 후보지역에서 제거            
        }

        // 표시용 오브젝트 삭제
        if (criticalMarks.ContainsKey(index))   // 키가 있는지 확인
        {
            Destroy(criticalMarks[index]);      // 오브젝트 제거
            criticalMarks[index] = null;        // 제거했다고 표시
            criticalMarks.Remove(index);        // 키값 제거
        }
    }

    /// <summary>
    /// 모든 우선 순위가 높은 후보지역을 제거
    /// </summary>
    private void RemoveAllCriticalPositions()
    {
        while(criticalMarkParent.childCount > 0)    // 생성한 공격 후보지역 표시용 오브젝트 모두 삭제
        {
            Transform child = criticalMarkParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        criticalMarks.Clear();                      // 딕셔너리 초기화
        
        criticalAttackIndices.Clear();              // 공격 후보지역 리스트 초기화
        lastSuccessAttackPosition = NOT_SUCCESS;
    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 턴이 시작될 때 플레이어가 해야할 일을 수행하는 함수
    /// </summary>
    protected virtual void OnPlayerTurnStart(int _)
    {
        isActionDone = false;
    }

    /// <summary>
    /// 턴이 종료될 때 플레이어가 해야할 일을 수행하는 함수
    /// </summary>
    protected virtual void OnPlayerTurnEnd()
    {
        if(!isActionDone)   // 턴이 끝났는데도 아무행동을 안했으면 자동 공격
        {
            AutoAttack();
        }
    }

    // 함선 침몰 빛 패배처리 함수 -------------------------------------------------------------------
    
    /// <summary>
    /// 함선이 침몰했을 때 실행되는 함수
    /// </summary>
    /// <param name="ship">침몰된 함선</param>
    void OnShipDestroy(Ship ship)
    {
        opponent.opponentShipDestroyed = true;              // 상대방에게 (상대방의 상대방(나)) 함선이 침몰되었다고 표시
        opponent.lastSuccessAttackPosition = NOT_SUCCESS;   // 상대방의 마지막 공격 성공 위치도 초기화(함선이 침몰했으니 의미없음)

        remainShipCount--;
        if(remainShipCount < 1)
        {
            OnDefeat();
        }
    }

    /// <summary>
    /// 모든 함선이 침몰했을 때 실행되는 함수
    /// </summary>
    protected virtual void OnDefeat()
    {
        Debug.Log($"[{gameObject.name}] 패배");
        onDefeat?.Invoke(this);
    }

    // 기타 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어 초기화 함수. 게임 시작 직전 상태로 만들기.
    /// </summary>
    public void Clear()
    {
        opponentShipDestroyed = false;
        Board.ResetBoard(Ships);
    }

    // 테스트 --------------------------------------------------------------------------------------
#if UNITY_EDITOR
    public void Test_IsSuccessLine(Vector2Int grid)
    {
        if( IsSuccessLine(grid, lastSuccessAttackPosition, true) )
        {
            Debug.Log("수평으로 공격이 성공했습니다.");
        }
        else if (IsSuccessLine(grid, lastSuccessAttackPosition, false))
        {
            Debug.Log("수직으로 공격이 성공했습니다.");
        }
        else
        {
            Debug.Log("한 줄이 아닙니다.");
        }
    }
#endif
}
