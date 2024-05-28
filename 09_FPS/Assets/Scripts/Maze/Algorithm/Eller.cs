using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerCell : Cell
{
    public int setGroup;
    const int NotSet = -1;
    public EllerCell(int x, int y) : base(x, y)
    {
        setGroup = NotSet;
    }
}

public class Eller : Maze
{
    /// <summary>
    /// 일반적으로 이웃 셀과 합쳐질 확률
    /// </summary>
    [Range(0f, 1f)]
    public float mergeChance = 0.7f;
    
    /// <summary>
    /// 고유한 집합을 설정하기 위한 시리얼 넘버
    /// </summary>
    int serial = 0;

    protected override void OnSpecificAlgorithmExcute()
    {
        /// 1. 한 줄 만들기
        ///     1.1. 위쪽 줄을 참조해서 한 줄 만들기
        ///         1.1.1. 위쪽 셀에 벽이 없으면 위쪽 셀과 같은 집합에 포함시킨다.
        ///         1.1.2. 위쪽 셀에 벽이 있으면 새 집합에 포함시킨다.
        ///     1.2. 첫번째 줄은 가로 길이만큼 셀을 만들고 각각 고유한 집합에 포함시킨다.
        /// 2. 옆칸끼리 합치기
        ///     2.1. 서로 집합이 다르면 랜덤하게 벽을 제거하고 같은 집합으로 만든다.(같은 줄에 있는 같은 종류의 셀이 한번에 바뀐다.)
        ///     2.2. 서로 같은 집합이면 패스
        /// 3. 아래쪽 벽 제거하기
        ///     3.1. 같은 집합당 최소 하나 이상의 벽이 제거되어야 한다.
        /// 4. 한 줄 완료 -> 1번으로 돌아가기
        /// 5. 마지막 줄 만들고 정리
        ///     5.1. 생성까진 똑같이 처리
        ///     5.2. 합칠 때 세트가 다르면 무조건 합친다.


        int h = height - 1;     // 미리 계산해 놓기
        EllerCell[] prevLine = null;    // 이전 줄을 저장하는 변수
        
        for (int y = 0;y<h;y++) // 첫줄부터 마지막줄까지 만드는 용도의 for
        {
            // 1. 한 줄 만들기
            EllerCell[] line = MakeLine(prevLine);

            // 2. 옆칸끼리 합치기
            MergeAdjacent(line, mergeChance);

            // 3. 아래쪽 벽 제거하기
            RemoveSouthWall(line);
                        
            WriteLine(line);    // 만든 줄 기록하기
            prevLine = line;    // 만든 줄을 이전 줄로 설정하기

            // 4. 한줄 완료
        }

        // 5. 마지막 줄 만들기
        EllerCell[] lastLine = MakeLine(prevLine);
        const float LastMergeChange = 1.1f;         // 마지막 줄에서 이웃 셀과 합쳐질 확률(무조건 합쳐져야 함)
        MergeAdjacent(lastLine, LastMergeChange);
        WriteLine(lastLine);
    }

    /// <summary>
    /// 한 줄을 만드는 함수
    /// </summary>
    /// <param name="prev">이전 줄</param>
    /// <returns>새롭게 만들어진 한 줄</returns>
    EllerCell[] MakeLine(EllerCell[] prev)
    {
        return null;
    }

    /// <summary>
    /// 이웃 셀끼리 합치는 함수
    /// </summary>
    /// <param name="line">합치는 작업을 할 줄</param>
    /// <param name="chance">합쳐질 확률</param>
    void MergeAdjacent(EllerCell[] line, float chance)
    {

    }

    /// <summary>
    /// 각 집합별로 랜덤하게 하나 이상의 남쪽벽을 제거하는 함수
    /// </summary>
    /// <param name="line">작업 처리를 할 줄</param>
    void RemoveSouthWall(EllerCell[] line)
    {

    }

    /// <summary>
    /// 한 줄을 Maze.cells에 저장하는 함수
    /// </summary>
    /// <param name="line">저장할 줄</param>
    void WriteLine(EllerCell[] line)
    {

    }
}
