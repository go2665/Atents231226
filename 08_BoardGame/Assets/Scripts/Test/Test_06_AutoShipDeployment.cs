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

    /// <summary>
    /// 아직 배치되지 않는 배를 모두 자동으로 배치하는 함수
    /// </summary>
    void AutoShipDeployment()
    {
        Debug.Log("함선 자동 배치 실행");

        int maxCapacity = Board.BoardSize * Board.BoardSize;
        List<int> high = new List<int>(maxCapacity);
        List<int> low = new List<int>(maxCapacity);

        // 가장자리 부분은 low에 넣고 남은 부분은 high에 넣기
        for(int i=0;i<maxCapacity; i++)
        {
            if( (i % Board.BoardSize == 0)                          // 0, 10, 20, 30, 40, 50, 60, 70, 80, 90
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
        foreach (var ship in testShips)
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
        foreach (var ship in testShips)
        {
            if(!ship.IsDeployed)        // 배치되어 있는 경우만 처리
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
                    if(fail)
                    {
                        // 함선 머리 부분이 배치불가능하면 high에 되돌리기
                        high.Add(head);
                    }
                    else
                    {
                        // 함선의 머리 부분이 배치 가능하면 남은 부분도 high에 있는지 확인
                        for(int i=1;i<shipPositions.Length;i++)
                        {
                            int body = board.GridToIndex(shipPositions[i]).Value;
                            if(!high.Contains(body))
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
                while(fail && count < 1000)
                {
                    int head = low[0];      // low의 첫번째 아이템 꺼내기
                    low.RemoveAt(0);
                    grid = board.IndexToGrid((uint)head);       // 함선 머리 부분의 그리드 좌표 구하기
                    fail = !board.IsShipDeploymentAvailable(ship, grid, out shipPositions); // 배치 가능한지 확인
                    if(fail)
                    {
                        low.Add(head);      // 배치가 불가능하면 low에 되돌리기
                    }
                    count++;    // 시도회수 증가
                }

                // high와 low 모두에서 실패한 경우
                if(fail)
                {
                    Debug.LogWarning("함선 자동배치 실패!");
                    return;
                }

                // 실제 배치
                board.ShipDeployment(ship, grid);
                ship.gameObject.SetActive(true);

                // 배치된 위치를 high와 low에서 제거
                List<int> tempList = new List<int>(shipPositions.Length);
                foreach (var pos in shipPositions)
                {
                    tempList.Add(board.GridToIndex(pos).Value);
                }
                foreach(var index in tempList)
                {
                    high.Remove(index);
                    low.Remove(index);
                }

                // 배치된 함선 주변 위치를 low로 보내기
                List<int> toLow = GetShipAroundPositions(ship);
                foreach (var index in toLow)
                {
                    if(high.Contains(index))    // high에 있으면 
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

        if (ship.Direction == ShipDirection.North ||  ship.Direction == ShipDirection.South)
        {
            // 함선이 세로로 있는 경우

            // 함선의 옆면 두 줄 넣기
            foreach(var pos in ship.Positions)
            {
                index = board.GridToIndex(pos + Vector2Int.left);   // 보드 안쪽일 경우만 추가하기
                if (index.HasValue) result.Add(index.Value);

                index = board.GridToIndex(pos + Vector2Int.right);
                if (index.HasValue) result.Add(index.Value);
            }

            // 함선의 머리 위쪽, 꼬리 아래쪽
            Vector2Int head;
            Vector2Int tail;

            if(ship.Direction == ShipDirection.North)
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