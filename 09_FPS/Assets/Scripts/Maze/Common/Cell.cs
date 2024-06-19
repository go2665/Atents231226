using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Direction : byte
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
}

public class Cell
{
    /// <summary>
    /// 이 셀에 연결된 길을 기록하는 변수(북동남서 순서대로 해당하는 비트에 1이 세팅되어 있으면 길이 있다. 0이 세팅되어있으면 길이 없다(벽이 있다).)
    /// </summary>
    byte path;

    /// <summary>
    /// 연결한 길을 확인하기 위한 프로퍼티
    /// </summary>
    public byte Path => path;

    /// <summary>
    /// 미로 그리드 상에서의 x좌표(왼쪽->오른쪽)
    /// </summary>
    protected int x;
    public int X => x;

    /// <summary>
    /// 미로 그리드 상에서 y좌표(위 -> 아래)
    /// </summary>
    protected int y;
    public int Y => y;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="x">셀의 위치</param>
    /// <param name="y">셀의 위치</param>
    public Cell(int x, int y)
    {
        this.path = (byte)Direction.None;
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 이 셀에 길을 추가하는 함수
    /// </summary>
    /// <param name="pathDirection">새로 길을 만들 방향</param>
    public void MakePath(Direction pathDirection)
    {
        path |= (byte)pathDirection;
    }

    /// <summary>
    /// 특정 방향이 길인지 확인하는 함수
    /// </summary>
    /// <param name="pathDirection">확인할 방향</param>
    /// <returns>true면 길이고, false면 벽</returns>
    public bool IsPath(Direction pathDirection)
    {
        return (path & (byte)pathDirection) != 0;
    }

    /// <summary>
    /// 특정 방향이 벽인지 확인하는 함수
    /// </summary>
    /// <param name="pathDirection">확인할 방향</param>
    /// <returns>true면 벽이고, false면 길</returns>
    public bool IsWall(Direction pathDirection) 
    { 
        return (path & (byte)pathDirection) == 0; 
    }

    /// <summary>
    /// 코너 체크용 함수(양 방향에 모두 길이 있어야 true)
    /// </summary>
    /// <param name="dir1"></param>
    /// <param name="dir2"></param>
    /// <returns>dir1과 dir2가 코너를 만드는 방향이고 둘 다 길이 있을 때만 true</returns>
    public bool CornerPathCheck(Direction dir1, Direction dir2)
    {
        bool result = false;
        Direction corner = dir1 | dir2;
        if (corner == (Direction.North | Direction.East)
            || corner == (Direction.North | Direction.West)
            || corner == (Direction.South | Direction.East)
            || corner == (Direction.South | Direction.West))    // 코너 인지 확인
        {
            result = IsPath(dir1) && IsPath(dir2);  // 양쪽다 길이 있는지 확인
        }

        return result;
    }

}
