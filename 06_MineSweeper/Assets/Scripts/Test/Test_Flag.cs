using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Flag : TestBase
{
    Board board;

    private void Start()
    {
        //board = FindAnyObjectByType<Board>();
        //board.Initialize(8, 8, 10);
    }
}

// 보드가 초기화 될 때 mineCount가 FlagCounter에 설정된다.
// 보드에 깃발을 설치하면 FlagCounter가 감소한다.
// 보드에서 깃발 설치를 해제하면 FlagCounter가 증가한다.