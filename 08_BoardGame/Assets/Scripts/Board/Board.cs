using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector2Int IndexToGrid(int index)
    {
        return Vector2Int.zero;
    }

    /// <summary>
    /// 인덱스 값을 월드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="index">인덱스 값</param>
    /// <returns>계산된 월드 좌표</returns>
    public Vector3 IndexToWorld(int index)
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 그리드 좌표를 배열의 인덱스 값으로 변경하는 함수
    /// </summary>
    /// <param name="grid">그리드 값</param>
    /// <returns>그리드 좌표가 보드 안이면 해당하는 인덱스, 아니면 null</returns>
    public int? GridToIndex(Vector2Int grid)
    {
        return null;
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="x">그리드의 x좌표</param>
    /// <param name="y">그리드의 y좌표</param>
    /// <returns>계산된 월드좌표</returns>
    public Vector3 GridToWorld(int x, int y)
    {
        return Vector3.zero;
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="world">월드 좌표</param>
    /// <returns>계산된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 world)
    {
        return Vector2Int.zero;
    }    

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="world">월드좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector3 world)
    {
        return false;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="x">그리드 x좌표</param>
    /// <param name="y">그리드 y좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(int x, int y)
    {
        return false;
    }

    /// <summary>
    /// 입력된 좌표가 보드 안인지 밖인지 확인하는 함수
    /// </summary>
    /// <param name="grid">그리드 좌표</param>
    /// <returns>true면 보드 안, false면 보드 밖</returns>
    public bool IsInBoard(Vector2Int grid)
    {
        return false;
    }

    /// <summary>
    /// 마우스 커서 위치의 그리드 좌표
    /// </summary>
    /// <returns>그리드 좌표</returns>
    public Vector2Int GetMouseGridPosition()
    {
        return Vector2Int.zero;
    }
}
