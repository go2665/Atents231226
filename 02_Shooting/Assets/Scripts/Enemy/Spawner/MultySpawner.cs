using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultySpawner : MonoBehaviour
{
    // 인스펙터 창에서 맴버변수인 구조체나 클래스 내부를 확인하고 싶으면 Serializable 속성을 추가해야 한다.
    [System.Serializable]
    public struct SpawnData
    {
        public SpawnData(PoolObjectType type = PoolObjectType.EnemyWave, float interval = 0.5f)
        {
            this.spawnType = type;
            this.interval = interval;
        }

        public PoolObjectType spawnType;
        public float interval;
    }

    public SpawnData[] spawnDatas;


    const float MinY = -4.0f;
    const float MaxY = 4.0f;

    Transform asteroidDestination;

    private void Awake()
    {
        asteroidDestination = transform.GetChild(0);
    }

    private void Start()
    {
        foreach (var data in spawnDatas)
        {
            StartCoroutine(SpawnCoroutine(data));
        }
    }

    private IEnumerator SpawnCoroutine(SpawnData data)
    {
        while(true)
        {
            yield return new WaitForSeconds(data.interval);
            float height = Random.Range(MinY, MaxY);

            GameObject obj = Factory.Instance.GetObject(data.spawnType, new(transform.position.x, height, 0.0f));

            switch (data.spawnType)
            {
                case PoolObjectType.EnemyAsteroid:
                    Asteroid asteroid = obj.GetComponent<Asteroid>();
                    asteroid.SetDestination(GetDestination());
                    break;
            }
        }
    }

    /// <summary>
    /// 운석의 목적지를 랜덤으로 구해주는 함수
    /// </summary>
    /// <returns>랜덤한 목적지</returns>
    Vector3 GetDestination()
    {
        Vector3 pos = asteroidDestination.position;
        pos.y += Random.Range(MinY, MaxY);  // 현재 위치에서 높이만 (-4 ~ +4) 변경

        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;                             // 색깔 지정
        Vector3 p0 = transform.position + Vector3.up * MaxY;    // 선의 시작점 계산
        Vector3 p1 = transform.position + Vector3.up * MinY;    // 선의 도착점 계산
        Gizmos.DrawLine(p0, p1);                                // 시작점에서 도착점으로 선을 긋는다.

        if (asteroidDestination == null)
        {
            asteroidDestination = transform.GetChild(0);
        }
        Gizmos.color = Color.yellow;                             // 색깔 지정
        Vector3 p2 = asteroidDestination.position + Vector3.up * MaxY;    // 선의 시작점 계산
        Vector3 p3 = asteroidDestination.position + Vector3.up * MinY;    // 선의 도착점 계산
        Gizmos.DrawLine(p2, p3);                                // 시작점에서 도착점으로 선을 긋는다.
    }

    private void OnDrawGizmosSelected()
    {
        // 이 오브젝트를 선택했을 때 사각형 그리기(색상 변경하기)

        Gizmos.color = Color.red;                             // 색깔 지정        
        Vector3 p0 = transform.position + Vector3.up * MaxY - Vector3.right * 0.5f;
        Vector3 p1 = transform.position + Vector3.up * MaxY + Vector3.right * 0.5f;
        Vector3 p2 = transform.position + Vector3.up * MinY + Vector3.right * 0.5f;
        Vector3 p3 = transform.position + Vector3.up * MinY - Vector3.right * 0.5f;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        if (asteroidDestination == null)
        {
            asteroidDestination = transform.GetChild(0);
        }
        Gizmos.color = Color.red;                             // 색깔 지정        
        Vector3 p4 = asteroidDestination.position + Vector3.up * MaxY - Vector3.right * 0.5f;
        Vector3 p5 = asteroidDestination.position + Vector3.up * MaxY + Vector3.right * 0.5f;
        Vector3 p6 = asteroidDestination.position + Vector3.up * MinY + Vector3.right * 0.5f;
        Vector3 p7 = asteroidDestination.position + Vector3.up * MinY - Vector3.right * 0.5f;
        Gizmos.DrawLine(p4, p5);
        Gizmos.DrawLine(p5, p6);
        Gizmos.DrawLine(p6, p7);
        Gizmos.DrawLine(p7, p4);
    }


}
