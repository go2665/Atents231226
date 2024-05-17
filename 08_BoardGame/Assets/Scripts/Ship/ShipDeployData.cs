using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 함선 하나에 대한 배치 정보를 저장하는 클래스
/// </summary>
public class ShipDeployData
{
    /// <summary>
    /// 배치된 방향
    /// </summary>
    ShipDirection direction;
    public ShipDirection Direction => direction;

    /// <summary>
    /// 배치된 위치
    /// </summary>
    Vector2Int position;
    public Vector2Int Position => position;

    /// <summary>
    /// 배치 정보 클래스의 생성자
    /// </summary>
    /// <param name="direction">방향</param>
    /// <param name="position">위치</param>
    public ShipDeployData(ShipDirection direction, Vector2Int position)
    {
        this.direction = direction;
        this.position = position;
    }
}
