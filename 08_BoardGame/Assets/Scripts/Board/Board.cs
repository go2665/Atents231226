using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    // 각종 유틸리티 함수들(위치 관련)
    public const int BoardSize = 10;

    // 좌표 변환용 유틸리티 함수들-------------------------------------------------------------------------------------

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>보드 상의 그리드 위치</returns>
    public Vector2Int IndexToGrid(uint index)
    {
        return new Vector2Int((int)index % BoardSize, (int)index / BoardSize);
    }

    /// <summary>
    /// 인덱스 값을 월드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>계산된 월드 좌표</returns>
    public Vector3 IndexToWorld(uint index)
    {
        return GridToWorld(IndexToGrid(index));
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변경하는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <returns>그리드 좌표가 보드 안이면 해당하는 인덱스, 아니면 null</returns>
    public int? GridToIndex(int x, int y)
    {
        int? result = null;
        if( IsInBoard(x,y) )
        {
            result = x + y * BoardSize;
        }
        return result;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변경하는 함수
    /// </summary>
    /// <param name="grid">그리드 값</param>
    /// <returns>그리드 좌표가 보드 안이면 해당하는 인덱스, 아니면 null</returns>
    public int? GridToIndex(Vector2Int grid)
    {
        return GridToIndex(grid.x, grid.y);
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <returns>계산된 월드좌표(그리드의 중심점)</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0, -(y + 0.5f));
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>계산된 월드좌표(그리드의 중심점)</returns>
    public Vector3 GridToWorld(Vector2Int grid)
    {
        return GridToWorld(grid.x, grid.y);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="world">월드 좌표</param>
    /// <returns>계산된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 world)
    {
        world.y = transform.position.y; // x,z의 차이만 구하기 위해 y는 보드의 위치와 일치시키기

        Vector3 diff = world - transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x), Mathf.FloorToInt(-diff.z));
    }    

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="world">월드좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector3 world)
    {
        world.y = transform.position.y;
        Vector3 diff = world - transform.position;

        return diff.x >= 0 && diff.x <= BoardSize && diff.z <= 0 && diff.z >= -BoardSize;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(int x, int y)
    {
        return x > -1 && x < BoardSize && y > -1 && y < BoardSize;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector2Int grid)
    {
        return IsInBoard(grid.x, grid.y);
    }

    /// <summary>
    /// 마우스 커서 위치의 그리드 좌표
    /// </summary>
    /// <returns>그리드 좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);        
        return WorldToGrid(world);
    }
}
