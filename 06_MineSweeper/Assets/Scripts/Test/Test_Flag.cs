using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Flag : TestBase
{
    Board board;

    private void Start()
    {
        board = FindAnyObjectByType<Board>();
        board.Initialize(8, 8, 10);
    }
}

// 닫혀있는 셀을 우클릭할 때마다 커버의 모양이 변경되어야 한다.
// 닫혀 있는 셀의 상태 : None, Flag, Question
//      None -> Flag
//      Flag -> Question
//      Question -> None