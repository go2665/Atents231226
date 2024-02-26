using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_AStar_Tilemap : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;

    protected override void OnTestLClick(InputAction.CallbackContext _)
    {
        // 타일맵의 그리드 좌표 구하기
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPostion = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int)background.WorldToCell(worldPostion);

        Debug.Log(gridPosition);    
    }

    protected override void OnTestRClick(InputAction.CallbackContext _)
    {
        // 클릭한 위치에 타일이 있는지 없는지 확인
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPostion = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int gridPosition = (Vector2Int)obstacle.WorldToCell(worldPostion);

        TileBase tile = obstacle.GetTile((Vector3Int)gridPosition);
        if (tile != null)
        {
            Debug.Log("타일 있음");
        }
        else
        {
            Debug.Log("타일 없음");
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //background.size.x;  // background의 가로에 들어있는 셀의 개수(가로길이), 지워도 늘어난 size는 줄지 않는다.
        Debug.Log($"background : {background.size.x}, {background.size.y}");
        Debug.Log($"obstacle : {obstacle.size.x}, {obstacle.size.y}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // background.origin : background에 있는 셀 중에 왼쪽 아래가 원점
        Debug.Log($"background origin : {background.origin}");
        Debug.Log($"obstacle origin : {obstacle.origin}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // cellBounds.min : 가장 왼쪽 아래의 좌표
        // cellBounds.max : 가장 오른쪽 위의 좌표
        Debug.Log($"background : {background.cellBounds.min}, {background.cellBounds.max}");
        Debug.Log($"obstacle : {obstacle.cellBounds.min}, {obstacle.cellBounds.max}");
    }
}
