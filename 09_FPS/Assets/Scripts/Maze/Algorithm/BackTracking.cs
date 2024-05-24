using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrackingCell : Cell
{
    public BackTrackingCell(int x, int y) : base(x, y)
    {

    }
}

public class BackTracking : Maze
{    

    protected override void OnSpecificAlgorithmExcute()
    {
        // 재귀적 백트래킹 알고리즘(Recursive BackTracking Algorithm)
        // 1. 미로에서 랜덤한 지점을 미로에 임시로 추가한다.
        // 2. 마지막에 미로에 추가한 지점에서 갈 수 있는 방향 중 하나를 선택해서 랜덤하게 이동한다.
        // 3. 이동한 곳은 미로에 추가되고, 이전 지점과의 통로가 연결된다.
        // 4. 이동 할 곳이 없을 경우 이전 단계의 셀로 돌아간다.
        // 5. 시작지점까지 돌아가면 알고리즘 종료
    }

    void MakeRecursive(int x, int y)
    {

    }
}
