using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public bool visited;
    public BackTrackingCell(int x, int y) : base(x, y)
    {
        visited = false;
    }
}

public class BackTracking : Maze
{
    // 참조
    // https://weblog.jamisbuck.org/2010/12/27/mazeGenerator-generation-recursive-backtracking

    protected override void OnSpecificAlgorithmExcute()
    {
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new BackTrackingCell(x, y);  // 모든 셀 생성(종류에 맞는 cell로 생성)
            }
        }

        // 재귀적 백트래킹 알고리즘(Recursive BackTracking Algorithm)
        // 1. 미로에서 랜덤한 지점을 방문했다고 표시한다.
        // 2. 마지막에 미로에 추가한 지점에서 갈 수 있는 방향(이전에 방문하지 않은 곳) 중 하나를 선택해서 랜덤하게 이동한다.
        // 3. 이동한 곳은 방문했다고 표시되고, 이전 지점과의 통로가 연결된다. 그리고 다시 2번 수행하기
        // 4. 이동 할 곳이 없을 경우 이전 단계의 셀로 돌아간다. 그리고 다시 2번 수행하기
        // 5. 시작지점까지 돌아가면 알고리즘 종료

        // 랜덤하게 시작지점 고르기
        int index = Random.Range(0, cells.Length);
        BackTrackingCell start = (BackTrackingCell)cells[index];
        start.visited = true;

        // 재귀문 시작
        MakeRecursive(start.X, start.Y);

        // 시작지점까지 돌아왔으므로 알고리즘 종료
    }

    /// <summary>
    /// 재귀처리를 위한 함수
    /// </summary>
    /// <param name="x">cell의 x위치</param>
    /// <param name="y">cell의 y위치</param>
    void MakeRecursive(int x, int y)
    {
        BackTrackingCell current = (BackTrackingCell)cells[GridToIndex(x,y)];

        Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
        Util.Shuffle(dirs); // 랜덤하게 이동할 방향 결정

        foreach(Vector2Int dir in dirs) 
        {
            Vector2Int newPos = new(x + dir.x, y + dir.y);

            if( IsInGrid(newPos) )  // 미로 안인지 확인
            {
                BackTrackingCell neighbor = (BackTrackingCell)cells[GridToIndex(newPos)];
                if(!neighbor.visited)   // 방문한적 있는지 확인(방문하지 않았어야 함)
                {
                    neighbor.visited = true;        // 방문했다고 표시
                    ConnectPath(current, neighbor); // 두 셀간에 길을 연결

                    MakeRecursive(neighbor.X, neighbor.Y);
                }
            }
        }

        // 4방향 확인이 끝났다.
    }
}
