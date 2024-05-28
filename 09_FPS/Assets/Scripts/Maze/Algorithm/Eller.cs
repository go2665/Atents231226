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
    /// 집합당 아래쪽으로 벽을 제거할 확률
    /// </summary>
    [Range(0f, 1f)]
    public float southOpenChance = 0.5f;
    
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
        /// 1. 한 줄 만들기
        ///     1.1. 위쪽 줄을 참조해서 한 줄 만들기
        ///         1.1.1. 위쪽 셀에 벽이 없으면 위쪽 셀과 같은 집합에 포함시킨다.
        ///         1.1.2. 위쪽 셀에 벽이 있으면 새 집합에 포함시킨다.
        ///     1.2. 첫번째 줄은 가로 길이만큼 셀을 만들고 각각 고유한 집합에 포함시킨다.

        int row = (prev != null) ? (prev[0].Y + 1) : 0;

        EllerCell[] line = new EllerCell[Width];
        for(int x = 0;x<width;x++)
        {
            line[x] = new EllerCell(x, row);            // 새 셀 만들기

            if( prev != null && prev[x].IsPath(Direction.South))
            {
                // 위쪽 줄이 있고, 위쪽 줄에 남쪽 벽이 없다. => 위쪽 셀의 집합과 같은 집합
                line[x].setGroup = prev[x].setGroup;    // 위쪽 셀과 같은 집합에 속하게 만들기
                line[x].MakePath(Direction.North);      // 위쪽으로 길을 만들기(위쪽 셀이 남쪽으로 길이 나있기 때문에)
            }
            else
            {
                // 위쪽 줄이 없거나, 위쪽 줄에 남쪽 벽이 있다. => 유니크한 집합에 포함
                line[x].setGroup = serial;              // 고유한 집합에 속하게 만들기
                serial++;                               // 다음 고유한 값 만들기
            }
        }

        return line;
    }

    /// <summary>
    /// 이웃 셀끼리 합치는 함수
    /// </summary>
    /// <param name="line">합치는 작업을 할 줄</param>
    /// <param name="chance">합쳐질 확률</param>
    void MergeAdjacent(EllerCell[] line, float chance)
    {
        /// 2. 옆칸끼리 합치기
        ///     2.1. 서로 집합이 다르면 랜덤하게 벽을 제거하고 같은 집합으로 만든다.(같은 줄에 있는 같은 종류의 셀이 한번에 바뀐다.)
        ///     2.2. 서로 같은 집합이면 패스
        ///     

        int count = 1;          // 한줄이 모두 같은 집합에 속하는 것을 방지하기 위한 카운터
        int w = width - 1;      // 미리 계산해 놓은 것
        for (int x = 0; x<w;x++)
        {
            if (count < w && line[x].setGroup != line[x+1].setGroup && Random.value < chance)
            {
                // count가 width보다 작다 = 한줄이 모두 같은 집합에 속하지 않는다.
                // x와 x+1번째의 셀이 같은 그룹에 속하지 않는다
                // 설정한 확률을 통과했다.

                line[x].MakePath(Direction.East);       // 서로 길을 만들기
                line[x+1].MakePath(Direction.West);

                int targetGroup = line[x + 1].setGroup;     // x+1번째의 집합을 저장해 놓기
                line[x + 1].setGroup = line[x].setGroup;    // x+1번째를 x번째의 집합에 속하게 만들기
                for(int i = x+2; i<width;i++)
                {
                    if (line[i].setGroup == targetGroup)    // x+1번째와 같은 집합에 속한 셀들을 x번째의 집합에 속하게 만들기
                    {
                        line[i].setGroup = line[x].setGroup;
                    }
                }

                count++;    // 카운트 증가
            }
        }
    }

    /// <summary>
    /// 각 집합별로 랜덤하게 하나 이상의 남쪽벽을 제거하는 함수
    /// </summary>
    /// <param name="line">작업 처리를 할 줄</param>
    void RemoveSouthWall(EllerCell[] line)
    {
        // 집합별로 리스트 만들기

        // 키 : 집합번호, 값 : 이 줄의 셀 중 키값에 해당하는 집합에 포함되는 셀의 x좌표
        Dictionary<int, List<int>> setListDic = new Dictionary<int, List<int>>();   
        for (int x = 0;x<Width;x++)
        {
            int key = line[x].setGroup;
            if (!setListDic.ContainsKey(key))
            {
                setListDic[key] = new List<int>();
            }
            setListDic[key].Add(x);
        }

        // 집합별 리스트를 배열로 변환하고, 집합별로 남쪽에 길만들기
        foreach(int key in setListDic.Keys)
        {
            int[] array = setListDic[key].ToArray();    // 배열로 변환하고
            Util.Shuffle(array);                        // 순서 섞기(랜덤하게 길만들기 위해)
            
            int index = array[0];                       // 첫번째는 무조건 아래쪽으로 길만들기
            line[index].MakePath(Direction.South);

            int length = array.Length;
            for(int i = 1; i < length; i++)             // 남은 것들은 확률에 따라 아래쪽에 길만들기
            {
                if(Random.value < southOpenChance)
                {
                    line[array[i]].MakePath(Direction.South);
                }
            }
        }
    }

    /// <summary>
    /// 한 줄을 Maze.cells에 저장하는 함수
    /// </summary>
    /// <param name="line">저장할 줄</param>
    void WriteLine(EllerCell[] line)
    {
        int index = GridToIndex(0, line[0].Y);
        for(int x = 0; x<Width;x++)
        {
            cells[index+x] = line[x];
        }
    }
}
