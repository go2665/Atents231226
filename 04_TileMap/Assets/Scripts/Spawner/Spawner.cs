using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour
{
    /// <summary>
    /// 스폰 간격
    /// </summary>
    public float interval = 1.0f;

    /// <summary>
    /// 마지막 스폰에서 지난 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 스폰 영역의 크기(transform.position에서 오른쪽(x), 위쪽(y)만큼의 크기)
    /// </summary>
    public Vector2 size;

    /// <summary>
    /// 스포너에서 동시에 최대로 유지가능한 슬라임의 수
    /// </summary>
    public int capacity = 3;

    /// <summary>
    /// 현재 스폰된 슬라임 수
    /// </summary>
    int count = 0;

    /// <summary>
    /// 스폰이 될 수 있는 노드의 리스트
    /// </summary>
    List<Node> spawnAreaList;

    /// <summary>
    /// 그리드맵, 타일맵, 스포너를 관리하는 클래스
    /// </summary>
    MapArea map;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Start()
    {
        map = GetComponentInParent<MapArea>();
        spawnAreaList = map.CalcSpawnArea(this);
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if(count < capacity)            // 캐퍼시티 확인하고
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > interval)  // 인터벌 확인
            {
                Spawn();                // 둘 다 통과가 되면 스폰
                elapsedTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// 슬라임을 한마리 스폰하는 함수
    /// </summary>
    void Spawn()
    {
        if(IsSpawnAvailable(out Vector3 spawnPosition))     // 스폰을 할 수 있는지 확인(빈공간이 있어야 함)
        {
            Slime slime = Factory.Instance.GetSlime();      // 위치는 초기화함수에서 설정
            slime.Initialize(map.GridMap, spawnPosition);   // 초기화
            slime.onDie += () =>    // 죽었을 때 실행될 함수 등록
            {
                count--;    // count 감소
                player.MonsterKill(slime.lifeTimeBonus);
            };
            slime.transform.SetParent(transform);
            count++;
        }
    }

    /// <summary>
    /// 스폰이 가능한지 확인하고 가능하면 스폰가능한 위치를 돌려주는 함수
    /// </summary>
    /// <param name="spawnablePosision">스폰 가능한 위치(월드좌표)</param>
    /// <returns>true면 스폰 가능, false면 스폰 불가능</returns>
    bool IsSpawnAvailable(out Vector3 spawnablePosision)
    {
        bool result = false;
        List<Node> positions = new List<Node>();

        foreach(Node node in spawnAreaList)
        {
            if(node.nodeType == Node.NodeType.Plain)
            {
                positions.Add(node);        // 미리 맵에서 찾아놓은 벽이 아닌 지역 중에서 평지만 골라내기
            }
        }

        if(positions.Count > 0)
        {
            // 빈칸이 있다.
            int index = UnityEngine.Random.Range(0, positions.Count);
            Node target = positions[index];
            spawnablePosision = map.GridToWorld(target.X, target.Y);    // 빈칸 중 하나를 랜덤으로 골라 돌려주기
            result = true;  // 스폰 가능하다고 표시
        }
        else
        {
            // 빈칸이 없다. => 스폰 불가능
            spawnablePosision = Vector3.zero;
        }

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 스폰 영역 그려서 표현하기
        Vector3 p0 = new(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector3 p1 = p0 + Vector3.right * size.x;
        Vector3 p2 = p0 + (Vector3)size;
        Vector3 p3 = p0 + Vector3.up * size.y;

        Handles.color = Color.red;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }
#endif
}
