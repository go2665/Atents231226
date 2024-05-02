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
    /// 이 플레이어가 가지는 함선들
    /// </summary>
    protected Ship[] ships;
    public Ship[] Ships => ships;

    /// <summary>
    /// 대전 상대
    /// </summary>
    protected PlayerBase opponent;

    /// <summary>
    /// 함선 매니저
    /// </summary>
    protected ShipManager shipManager;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    protected GameManager gameManager;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        board = child.GetComponent<Board>();
    }

    protected virtual void Start()
    {
        shipManager = ShipManager.Instance;
        gameManager = GameManager.Instance;
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

            board.onShipAttacked[shipType] += ships[i].OnHitted;    // 공격 당했을 때 실행할 함수 연결
        }

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
        normalAttackIndice = new List<uint>(temp);  // 리스트로 만들기

        criticalAttackIndice = new List<uint>(10);  // 우선 순위가 높은 공격 후보지역 만들기(비어있음)

        lastSuccessAttackPosition = NOT_SUCCESS;    // 이전 공격이 성공한 적 없다고 초기화

    }

    // 함선배치 관련 함수 --------------------------------------------------------------------------
    /// <summary>
    /// 아직 배치되지 않는 배를 모두 자동으로 배치하는 함수
    /// </summary>
    /// <param name="isShipShips">true면 배치 후 함선 표시, false면 미표시</param>
    public void AutoShipDeployment(bool isShipShips)
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
                ship.gameObject.SetActive(isShipShips);

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
        if (opponentBoard.IsInBoard(attackGrid) && opponentBoard.IsAttackable(attackGrid))
        {
            //Debug.Log($"{attackPos} 공격");
            opponentBoard.OnAttacked(attackGrid);
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
    /// 일반 공격 후보 지역들의 인덱스들
    /// </summary>
    List<uint> normalAttackIndice;

    /// <summary>
    /// 우선 순위가 높은 공격 후보지역들의 인덱스들
    /// </summary>
    List<uint> criticalAttackIndice;

    /// <summary>
    /// 마지막으로 공격이 성공한 그리드 좌표. NOT_SUCCESS면 이전 공격은 실패한 것
    /// </summary>
    Vector2Int lastSuccessAttackPosition;

    /// <summary>
    /// 이전 공격이 성공하지 않았다고 표시하는 읽기 전용 변수
    /// </summary>
    readonly Vector2Int NOT_SUCCESS = -Vector2Int.one;

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
        if(criticalAttackIndice.Count > 0)      // 우선 순위가 높은 공격 후보 지역이 있는지 확인
        {
            target = criticalAttackIndice[0];   // 있는 것 꺼내기
            criticalAttackIndice.RemoveAt(0);
            normalAttackIndice.Remove(target);  // normal에서도 제거
        }
        else
        {
            target = normalAttackIndice[0];     // 우선 순위가 높은 공격 후보지역이 없으면 normal에서 꺼내기
            normalAttackIndice.RemoveAt(0);
        }

        Attack(target);

    }

    // 턴 관리용 함수 ------------------------------------------------------------------------------
    // 함선 침몰 빛 패배처리 함수 -------------------------------------------------------------------
    // 기타 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어 초기화 함수. 게임 시작 직전 상태로 만들기.
    /// </summary>
    public void Clear()
    {
        Board.ResetBoard(Ships);
    }

    // 테스트 --------------------------------------------------------------------------------------
}
