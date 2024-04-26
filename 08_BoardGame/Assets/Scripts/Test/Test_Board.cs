using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : TestBase
{
    public Board board;

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        // 디버그로 그리드 좌표 출력
        // 그 좌표가 보드의 안인지 밖인지 출력
        // 찍은 그리드의 중심점 출력
    }
}
