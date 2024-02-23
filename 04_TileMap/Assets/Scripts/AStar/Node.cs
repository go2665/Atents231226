using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    /// <summary>
    /// 그리드 맵에서의 x좌표
    /// </summary>
    private int x;
    public int X => x;

    /// <summary>
    /// 그리드 맵에서의 y좌표
    /// </summary>
    private int y;
    public int Y => y;

    /// <summary>
    /// A* 알고리즘의 G값(출발점에서 이 노드까지 오는데 걸린 실제 거리)
    /// </summary>
    public float G;

    /// <summary>
    /// A* 알고리즘의 H값(이 노드에서 도착점까지의 예상 거리)
    /// </summary>
    public float H;

    /// <summary>
    /// G와 H의 합(출발점에서 이 노드를 경유해서 도착점까지 이동할 때 예상되는 거리)
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// 노드가 가질 수 있는 종류들
    /// </summary>
    public enum NodeType
    {
        Plain,  // 평지(지나갈 수 있음)
        Wall,   // 벽(지나갈 수 없음)
        Slime   // 슬라임(지나갈 수 없음)
    }

    /// <summary>
    /// 이 노드의 종류
    /// </summary>
    public NodeType nodeType = NodeType.Plain;

    /// <summary>
    /// 경로상 앞에 있는 노드
    /// </summary>
    public Node parent;

    /// <summary>
    /// Node의 생성자
    /// </summary>
    /// <param name="x">그리드 맵에서의 x좌표</param>
    /// <param name="y">그리드 맵에서의 y좌표</param>
    /// <param name="nodeType">노드의 종류(기본값으로 평지)</param>
    public Node(int x, int y, NodeType nodeType = NodeType.Plain)
    {
        this.x = x;
        this.y = y;
        this.nodeType = nodeType;
        ClearData();
    }

    /// <summary>
    /// 길찾기를 할 때마다 초기화 시키기 위해 있는 함수
    /// </summary>
    public void ClearData()
    {
        G = float.MaxValue;     // g값은 비교해서 갱신되는 과정이 있으므로 기본값은 매우 커야 한다.
        H = float.MaxValue;
        parent = null;        
    }

    /// <summary>
    /// 같은 타입 간의 크기 비교를 하는 함수
    /// </summary>
    /// <param name="other">비교 대상</param>
    /// <returns>-1,0,1 셋 중 하나</returns>
    public int CompareTo(Node other)
    {
        // 나올 수 있는 리턴의 경우의 수
        // 0보다 작다(-1)  : 내가 작다(this < other)
        // 0과 같다        : 나와 같다(this == other)
        // 0보다 크다(+1)  : 내가 크다(this > other)

        if(other == null)               // other가 null이면 내가 크다
            return 1;

        return F.CompareTo(other.F);   // F 값을 기준으로 순서를 정해라
    }

    /// <summary>
    /// == 명령어 오버로딩, 두 노드가 같은지 확인(x,y가 같으면 같다)
    /// </summary>
    /// <param name="left">== 왼쪽에 있는 노드</param>
    /// <param name="right">== 오른쪽에 있는 노드</param>
    /// <returns>같으면 true, 다르면 false</returns>
    //public static bool operator ==(Node left, Node right)
    //{
    //    return left.x == right.x && left.y == right.y;
    //}


    public static bool operator ==(Node left, Vector2Int right)
    {
        return left.x == right.x && left.y == right.y;
    }

    /// <summary>
    /// != 명령어 오버로딩, 두 노드가 다른지 확인(x,y 중 하나가 다르면 다르다.)
    /// </summary>
    /// <param name="left">== 왼쪽에 있는 노드</param>
    /// <param name="right">== 오른쪽에 있는 노드</param>
    /// <returns>같으면 false, 다르면 true</returns>
    //public static bool operator !=(Node left, Node right)
    //{
    //    return left.x != right.x || left.y != right.y;
    //}

    public static bool operator !=(Node left, Vector2Int right)
    {
        return left.x != right.x || left.y != right.y;
    }

    public override bool Equals(object obj)
    {
        // obj는 Node 클래스고 this와 obj의 x, y가 같다.
        return obj is Node other && this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y); // 위치값 2개로 해쉬코드 만들기
    }
}
