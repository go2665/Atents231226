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
        Vector2Int grid = board.GetMouseGridPosition();
        Debug.Log($"Grid : ({grid.x}, {grid.y})");

        // 그 좌표가 보드의 안인지 밖인지 출력
        if(board.IsInBoard(grid))
        {
            Debug.Log("보드 안쪽");
        }
        else
        {
            Debug.Log("보드 바깥쪽");
        }

        // 찍은 그리드의 중심점(월드좌표) 출력
        Vector3 world = board.GridToWorld(grid);
        Debug.Log($"World : ({world.x}, {world.y}, {world.z})");

    }
}
