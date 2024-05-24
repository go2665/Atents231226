using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    protected int width;
    public int Width => width;
    protected int height;
    public int Height => height;


    protected Cell[] cells;
    public Cell[] Cells => cells;

    /// <summary>
    /// 미로를 생성하는 함수
    /// </summary>
    /// <param name="width">미로의 가로 길이</param>
    /// <param name="height">미로의 세로 길이</param>
    /// <param name="seed">랜덤용 시드. -1이아니면 지정된 시드 적용</param>
    /// <returns>만들어진 미로</returns>
    public Cell[] MakeMaze(int width, int height, int seed = -1)
    {
        this.width = width;
        this.height = height;

        if(seed != -1)
        {
            Random.InitState(seed);
        }

        cells = new Cell[Width * Height];

        OnSpecificAlgorithmExcute();    // 각 알고리즘별 코드 실행

        return cells;
    }

    /// <summary>
    /// 각 알고리즘별로 override해야하는 함수. 미로 생성 알고리즘 실행
    /// </summary>
    protected virtual void OnSpecificAlgorithmExcute()
    {
        // cell을 생성하고 알고리즘 결과에 맞게 세팅
    }

    /// <summary>
    /// 두 셀 사이의 벽을 제거하는 함수
    /// </summary>
    /// <param name="from">시작셀</param>
    /// <param name="to">도착셀</param>
    protected void ConnectPath(Cell from, Cell to)
    {

    }

    protected bool IsInGrid(int x, int y)
    {
        return false;
    }

    protected bool IsInGrid(Vector2Int grid)
    {
        return false;
    }

    protected Vector2Int IndexToGrid(int index)
    {
        return Vector2Int.zero;
    }

    protected int GridToIndex(int x, int y)
    {
        return -1;
    }

    protected int GridToIndex(Vector2Int grid)
    {
        return -1;
    }
}
