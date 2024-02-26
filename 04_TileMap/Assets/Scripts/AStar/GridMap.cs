using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 클래스
/// </summary>
public class GridMap
{
    /// <summary>
    /// 이 맵에 있는 모든 노드
    /// </summary>
    protected Node[] nodes;

    /// <summary>
    /// 맵의 가로 길이
    /// </summary>
    protected int width;

    /// <summary>
    /// 맵의 세로 길이
    /// </summary>
    protected int height;
    
    protected GridMap() 
    {        
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="width">가로길이</param>
    /// <param name="height">세로길이</param>
    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;

        nodes = new Node[width * height];   // 노드 배열 생성

        for(int y=0;y<height; y++)
        {
            for(int x=0;x<width;x++)
            {
                if(GridToIndex(x, y, out int? index))
                    nodes[index.Value] = new Node(x, y);    // 노드 생성
            }
        }
    }

    /// <summary>
    /// 모든 노드의 A* 계산용 데이터 초기화
    /// </summary>
    public void ClearMapData()
    {
        foreach(Node node in nodes)
        {
            node.ClearData();
        }
    }

    /// <summary>
    /// 특정 위치에 있는 노드를 리턴하는 함수
    /// </summary>
    /// <param name="x">맵에서의 x좌표</param>
    /// <param name="y">맵에서의 y좌표</param>
    /// <returns>찾은 노드</returns>
    public Node GetNode(int x, int y)
    {
        Node node = null;
        if( GridToIndex(x, y, out int? index) )
        {
            node = nodes[index.Value];
        }
        return node;
    }

    /// <summary>
    /// 특정 위치에 있는 노드를 리턴하는 함수
    /// </summary>
    /// <param name="gridPosition">맵에서의 그리드 좌표</param>
    /// <returns>찾은 노드</returns>
    public Node GetNode(Vector2Int gridPosition)
    {
        return GetNode(gridPosition.x, gridPosition.y);
    }

    /// <summary>
    /// 특정 위치가 평지인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">맵에서의 x좌표</param>
    /// <param name="y">맵에서의 y좌표</param>
    /// <returns>true면 평지, false면 평지 아님</returns>
    public bool IsPlain(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Plain;
    }

    /// <summary>
    /// 특정 위치가 평지인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="gridPosition">맵에서의 그리드 좌표</param>
    /// <returns>true면 평지, false면 평지 아님</returns>
    public bool IsPlain(Vector2Int gridPosition)
    {
        return IsPlain(gridPosition.x, gridPosition.y);
    }

    /// <summary>
    /// 특정 위치가 벽인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">맵에서의 x좌표</param>
    /// <param name="y">맵에서의 y좌표</param>
    /// <returns>true면 벽, false면 벽 아님</returns>
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Wall;
    }

    /// <summary>
    /// 특정 위치가 벽인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="gridPosition">맵에서의 그리드 좌표</param>
    /// <returns>true면 벽, false면 벽 아님</returns>
    public bool IsWall(Vector2Int gridPosition)
    {
        return IsWall(gridPosition.x,gridPosition.y);
    }

    /// <summary>
    /// 특정 위치가 슬라임인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">맵에서의 x좌표</param>
    /// <param name="y">맵에서의 y좌표</param>
    /// <returns>true면 슬라임, false면 슬라임 아님</returns>
    public bool IsSlime(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.nodeType == Node.NodeType.Slime;
    }

    /// <summary>
    /// 특정 위치가 슬라임인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="gridPosition">맵에서의 그리드 좌표</param>
    /// <returns>true면 슬라임, false면 슬라임 아님</returns>
    public bool IsSlime(Vector2Int gridPosition)
    {
        return IsSlime(gridPosition.x,gridPosition.y);
    }

    /// <summary>
    /// 그리드 좌표를 인덱스 값으로 변경해주는 함수
    /// </summary>
    /// <param name="x">맵에서의 x좌표</param>
    /// <param name="y">맵에서의 y좌표</param>
    /// <param name="index">(출력용)변경된 인덱스</param>
    /// <returns>좌표가 적절하면 true, 맵 밖이면 false</returns>
    protected bool GridToIndex(int x, int y, out int? index)
    {
        bool result = false;
        index = null;

        if (IsValidPosition(x,y))
        {
            index = CalcIndex(x,y);
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 인덱스 계산식
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>계산된 인덱스 값</returns>
    protected virtual int CalcIndex(int x, int y)
    {
        return x + y * width;
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">변경할 index값</param>
    /// <returns>변경된 그리드 좌표</returns>
    public Vector2Int IndexToGrid(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    /// <summary>
    /// 특정 위치가 맵 안인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 x좌표</param>
    /// <param name="y">확인할 y좌표</param>
    /// <returns>true면 맵 안, false면 맵 밖</returns>
    public virtual bool IsValidPosition(int x, int y)
    {
        return x < width && y < height && x >= 0 && y >= 0;
    }

    /// <summary>
    /// 특정 위치가 맵 안인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="gridPosition">확인할 그리드 좌표</param>
    /// <returns>true면 맵 안, false면 맵 밖</returns>
    public bool IsValidPosition(Vector2Int gridPosition)
    {
        return IsValidPosition(gridPosition.x, gridPosition.y);
    }
}
