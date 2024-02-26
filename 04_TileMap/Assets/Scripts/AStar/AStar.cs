using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class AStar
{
    const float sideDistance = 1.0f;
    const float diagonalDistance = 1.4f;

    /// <summary>
    /// 맵과 시작점과 도착점을 받아 경로를 계산하는 함수
    /// </summary>
    /// <param name="map">길을 찾을 맵</param>
    /// <param name="start">시작점</param>
    /// <param name="end">도착점</param>
    /// <returns>시작점에서 도착점까지의 경로, 길을 못찾으면 null</returns>
    public static List<Vector2Int> PathFine(GridMap map, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = null;

        if(map.IsValidPosition(start) && map.IsValidPosition(end) && map.IsPlain(start) && map.IsPlain(end))
        {
            // start와 end가 맵 안이고 평지다.

            map.ClearMapData(); // 맵 데이터 전체 초기화

            List<Node> open = new List<Node>();     // open list : 앞으로 탐색할 노드의 리스트
            List<Node> close = new List<Node>();    // close list : 탐색이 완료된 노드의 리스트

            // A* 알고리즘 시작하기
            Node current = map.GetNode(start);
            current.G = 0.0f;
            current.H = GetHeuristic(current, end);
            open.Add(current);

            // A* 루프 시작(핵심 루틴)
            while(open.Count > 0)   // open 리스트에 노드가 있으면 계속 반복
            {
                open.Sort();            // f값을 기준으로 정렬
                current = open[0];      // 제일 앞에 있는 노드(=f값이 가장 작은 노드)를 current로 설정
                open.RemoveAt(0);       // open리스트에서 제일 앞에 있는 노드를 제거

                if( current != end )
                {
                    // 목적지가 아니다.
                    close.Add(current); // close리스트에 current를 추가해서 탐색이 완료되었음을 표시

                    // current의 주변 8방향을 open 리스트에 추가
                    for(int y = -1;y<2;y++)
                    {
                        for(int x = -1;x<2;x++)
                        {
                            Node node = map.GetNode(current.X + x, current.Y + y);  // 주변 노드 가져오기

                            // 스킵할 노드인지 확인
                            if (node == null) continue;                             // 맵 밖
                            if (node == current) continue;                          // 자기자신
                            if (node.nodeType == Node.NodeType.Wall) continue;      // 벽
                            if (close.Exists((x) => x == node)) continue;           // close 리스트에 있음
                                                                                    // (close에 있는 모든 요소(x)를 node와 비교해서 하나라도 같으면 true, 전부 다르면 false)
                            // 대각선으로 가는데 옆에 벽이 있는 경우
                                                        

                        }
                    }
                }
                else
                {
                    // 목적지에 도착했다.
                    break;  // 목적지에 도착했으면 while 종료
                }

                //4.선택된 노드의 주변 노드를 open list에 추가한다.
                //(못가는 노드와 close list에 있는 노드는 하지 않음.g값이 이전보다 더 작은 경우는 갱신한다.)
                //5.선택된 노드는 close list에 들어간다.
                //6.선택된 노드가 도착점이 아니면 3번으로 돌아가 다시 실행한다.
            }

            // 마무리 작업(도착점에 도착했다 or 길을 못찾았다)
            if(current == end)
            {
                // 경로 만들기
            }

        }

        return path;
    }

    /// <summary>
    /// 휴리스틱 값을 계산하는 함수(현재 위치에서 목적지까지의 예상 거리)
    /// </summary>
    /// <param name="current">현재 노드</param>
    /// <param name="end">도착지점</param>
    /// <returns>예상 거리</returns>
    private static float GetHeuristic(Node current, Vector2Int end)
    {
        return Mathf.Abs(current.X - end.x) + Mathf.Abs(current.Y - end.y);
    }
}
