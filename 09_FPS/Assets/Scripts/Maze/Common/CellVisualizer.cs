using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour
{
    public const int CellSize = 10;

    GameObject[] walls;

    /// <summary>
    /// 입력받은 데이터에 맞게 벽의 활성화 여부 재설정
    /// </summary>
    /// <param name="data"></param>
    public void RefreshWall(byte data)
    {
        // data : 북동남서 순서대로 1이 세팅되어 있으면 길(=벽이 없다), 0이 세팅되어 있으변 벽.
    }

    /// <summary>
    /// 현재 셀의 상태를 확인해서 벽 활성화 정보를 리턴하는 함수
    /// </summary>
    /// <returns>4bit 북동남서 순으로, 벽은 0, 길은 1로 세팅되어 있는 데이터</returns>
    public Direction GetPath()
    {
        byte mask = 0;
        return (Direction)mask;
    }
}
