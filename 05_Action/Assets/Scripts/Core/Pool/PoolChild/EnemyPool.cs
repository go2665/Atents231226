using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<Enemy>
{
    /// <summary>
    /// 적들이 사용할 웨이포인트들. 반드시 하나는 있어야 한다.
    /// </summary>
    public Waypoints[] waypoints;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        waypoints = child.GetComponentsInChildren<Waypoints>(); // 풀의 자식에서 모두 찾기
    }

    /// <summary>
    /// 풀에서 사용하지 않는 오브젝트를 하나 꺼낸 후 리턴 하는 함수
    /// </summary>
    /// <param name="index">사용할 웨이포인트 인덱스</param>
    /// <param name="position">배치될 위치(월드좌표)</param>
    /// <param name="eulerAngle">배치될 때의 각도</param>
    /// <returns>풀에서 꺼낸 오브젝트(활성화됨)</returns>
    public Enemy GetObject(int index, Vector3? position = null, Vector3? eulerAngle = null)
    {
        Enemy enemy = GetObject(position, eulerAngle);      
        enemy.waypoints = waypoints[index];

        return enemy;
    }

    protected override void OnGenerateObject(Enemy comp)
    {
        comp.waypoints = waypoints[0];  // 디폴트로 첫번째 웨이포인트 사용
    }
}
