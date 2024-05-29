using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonCell : Cell
{
    /// <summary>
    /// 경로가 만들어 졌을 때 다음 셀의 참조
    /// </summary>
    public WilsonCell next;

    /// <summary>
    /// 이 셀이 미로에 포함되어 있는지 설정하고 확인하는 함수
    /// </summary>
    public bool isMazeMember;

    public WilsonCell(int x, int y) : base(x, y)
    {
        next = null;
        isMazeMember = false;
    }
}

public class Wilson : Maze
{
    readonly Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
    protected override void OnSpecificAlgorithmExcute()
    {
        // 1. 필드의 한곳을 랜덤으로 미로에 추가한다.
        // 2. 미로에 포함되지 않은 셀 중 하나를 랜덤으로 선택한다.(A셀)
        // 3. A셀의 위치에서 랜덤으로 한 칸 움직인다.(이동한 셀이 기록 되어야 한다)
        // 4. 미로에 포함된 셀에 도착할때까지 3번을 반복한다.
        // 5. A셀 위치에서 미로에 포함된 영역에 도착할 때까지의 경로를 미로에 포함시킨다.(경로에 따라 벽도 제거)
        // 6. 모든 셀이 미로에 포함될 때까지 2번으로 돌아가 반복한다.

        // 모든 셀 만들기
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                cells[GridToIndex(x, y)] = new WilsonCell(x, y);
            }
        }

        // 미로에 포함되지 않은 셀들을 기록한 리스트 만들기
        int[] notInMazeArray = new int[cells.Length];   // 섞기 위해서 배열 만들기
        for(int i=0;i<notInMazeArray.Length;i++)
        {
            notInMazeArray[i] = i;                      // 인덱스 기록하기
        }
        Util.Shuffle(notInMazeArray);                   // 배열 섞기
        List<int> notInMaze = new List<int>(notInMazeArray);    // 배열을 기반으로 리스트 만들기

        // 1. 필드의 한곳을 랜덤으로 미로에 추가한다.
        int firstIndex = notInMaze[0];                      // 미로에 포함되지 않은 셀 중에서 하나 꺼내기
        notInMaze.RemoveAt(0);
        WilsonCell first = (WilsonCell)cells[firstIndex];
        first.isMazeMember = true;                          // 꺼낸 셀을 미로에 포함시키기

        while (notInMaze.Count > 0) // notInMaze에 포함된 아이템의 개수가 0보다 크다 == 아직 미로에 포함되지 않은 셀이 남아있다.
        {
            // 2. 미로에 포함되지 않은 셀 중 하나를 랜덤으로 선택한다.(A셀)
            int index = notInMaze[0];
            notInMaze.RemoveAt(0);
            WilsonCell current = (WilsonCell)cells[index];

            // 3. A셀의 위치에서 랜덤으로 한 칸 움직인다.(이동한 셀이 기록 되어야 한다)
            do
            {
                WilsonCell neighbor = GetNeighbor(current); // 이웃셀을 구하고
                current.next = neighbor;                    // 어디로 이동하는지 기록
                current = neighbor;                         // current 변경
            } while (!current.isMazeMember);    // 4. 미로에 포함된 셀에 도착할때까지 3번을 반복한다.

            // 5. A셀 위치에서 미로에 포함된 영역에 도착할 때까지의 경로를 미로에 포함시킨다.(경로에 따라 벽도 제거)
            WilsonCell path = (WilsonCell)cells[index];
            while (path != current)  // 시작부터 current에 도착할 때까지 돌기
            {
                path.isMazeMember = true;                       // 이 셀을 미로에 포함시키기
                notInMaze.Remove(GridToIndex(path.X, path.Y));  // 미로에 포함되어 있지 않은 셀들의 목록에서 제거
                ConnectPath(path, path.next);                   // 다음 셀과 길 연결하고
                path = path.next;                               // 다음 셀로 넘어가기
            }
        }   // 6. 모든 셀이 미로에 포함될 때까지 2번으로 돌아가 반복한다.

    }

    /// <summary>
    /// 파라메터로 받은 셀의 이웃 중 하나를 리턴하는 함수
    /// </summary>
    /// <param name="cell">이웃을 찾을 셀</param>
    /// <returns>파라메터의 이웃 중 하나</returns>
    WilsonCell GetNeighbor(WilsonCell cell)
    {
        Vector2Int neighborPos;

        do
        {
            Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
            neighborPos = new Vector2Int(cell.X + dir.x, cell.Y + dir.y);
        } while (!IsInGrid(neighborPos));   // 그리드 영역 안에 있는 위치를 고를 때까지 반복

        return (WilsonCell)cells[GridToIndex(neighborPos)];
    }
}
