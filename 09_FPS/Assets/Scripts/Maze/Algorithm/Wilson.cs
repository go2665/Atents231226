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
    protected override void OnSpecificAlgorithmExcute()
    {
        // 1. 필드의 한곳을 랜덤으로 미로에 추가한다.
        // 2. 미로에 포함되지 않은 셀 중 하나를 랜덤으로 선택한다.(A셀)
        // 3. A셀의 위치에서 랜덤으로 한 칸 움직인다.(이동한 셀이 기록 되어야 한다)
        // 4. 미로에 포함된 셀에 도착할때까지 3번을 반복한다.
        // 5. A셀 위치에서 미로에 포함된 영역에 도착할 때까지의 경로를 미로에 포함시킨다.(경로에 따라 벽도 제거)
        // 6. 모든 셀이 미로에 포함될 때까지 2번으로 돌아가 반복한다.

    }
}
