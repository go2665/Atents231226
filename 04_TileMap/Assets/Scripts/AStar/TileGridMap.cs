using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridMap : GridMap
{
    /// <summary>
    /// 맵의 원점
    /// </summary>
    Vector2Int origin;

    /// <summary>
    /// 배경 타일맵(크기 확인 및 좌표계산용)
    /// </summary>
    Tilemap background;

    /// <summary>
    /// 이동 가능한 지역(평지)의 위치 모음
    /// </summary>
    Vector2Int[] movablePositions;

    public TileGridMap(Tilemap background, Tilemap obstacle)
    {
        this.background = background;
        width = background.size.x;         // background타일맵의 크기를 가로세로 길이로 지정
        height = background.size.y;

        origin = (Vector2Int)background.origin; // 원점 설정

        nodes = new Node[width * height];       // 노드 배열 생성

        Vector2Int min = (Vector2Int)background.cellBounds.min; // for를 위한 최소/최대값 구하기
        Vector2Int max = (Vector2Int)background.cellBounds.max;

        List<Vector2Int> movable = new List<Vector2Int>(width * height);    // 이동 가능한 지역을 저장할 임시 리스트
        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (GridToIndex(x, y, out int? index))              // 인덱스값 구하기
                {
                    Node.NodeType nodeType = Node.NodeType.Plain;
                    TileBase tile = obstacle.GetTile(new(x, y));
                    if(tile != null)
                    {
                        nodeType = Node.NodeType.Wall;              // 장애물 타일이 있는 곳이면 벽으로 설정
                    }
                    else
                    {
                        movable.Add(new(x, y));                     // 이동 가능한 지역 저장
                    }

                    nodes[index.Value] = new Node(x, y, nodeType);  // 인덱스 위치에 노드 생성
                }
            }
        }

        movablePositions = movable.ToArray();   // 임시 리스트를 배열로 저장
    }

    /// <summary>
    /// 타일맵을 사용했을 때의 인덱스 계산식
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>계산된 인덱스 값</returns>
    protected override int CalcIndex(int x, int y)
    {
        // 원래 수식 : x + y * width;                       
        // 원점이 (0,0)이 아닐 때에 대한 처리 : (x - origin.x) + (y - origin.y) * width;
        // y가 뒤집힌 것을 위한 처리 : (x - origin.x) + ((height - 1) - (y - origin.y)) * width;

        return (x - origin.x) + ((height - 1) - (y - origin.y)) * width;
    }

    /// <summary>
    /// 특정 위치가 타일맵 안인지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 x좌표</param>
    /// <param name="y">확인할 y좌표</param>
    /// <returns>true면 맵 안, false면 맵 밖</returns>
    public override bool IsValidPosition(int x, int y)
    {
        // 원래 식 : x < width && y < height && x >= 0 && y >= 0;
        return x < (width + origin.x) && y < (height + origin.y) && x >= origin.x && y >= origin.y;
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="worldPosition">월드좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return (Vector2Int)background.WorldToCell(worldPosition);
    }

    /// <summary>
    /// 그리드 좌표를 월드좌표로 변경하는 함수
    /// </summary>
    /// <param name="gridPosition">그리드 좌표</param>
    /// <returns>그리드의 가운데 지점에 해당하는 월드좌표</returns>
    public Vector2 GridToWorld(Vector2Int gridPosition)
    {
        return background.CellToWorld((Vector3Int)gridPosition) + new Vector3(0.5f,0.5f);
    }

    /// <summary>
    /// 이동 가능한 위치 중 랜덤으로 선택해서 리턴하는 함수
    /// </summary>
    /// <returns>이동가능한 위치</returns>
    public Vector2Int GetRandomMoveablePosition()
    {
        int index = UnityEngine.Random.Range(0, movablePositions.Length);
        return movablePositions[index];
    }
}
