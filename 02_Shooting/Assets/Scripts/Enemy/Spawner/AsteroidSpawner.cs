using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : EnenySpawner
{
    Transform destinationArea;

    private void Awake()
    {
        destinationArea = transform.GetChild(0);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(destinationArea == null ) 
        {
            destinationArea = transform.GetChild(0);
        }
        Gizmos.color = Color.yellow;                             // 색깔 지정
        Vector3 p0 = destinationArea.position + Vector3.up * MaxY;    // 선의 시작점 계산
        Vector3 p1 = destinationArea.position + Vector3.up * MinY;    // 선의 도착점 계산
        Gizmos.DrawLine(p0, p1);                                // 시작점에서 도착점으로 선을 긋는다.
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (destinationArea == null)
        {
            destinationArea = transform.GetChild(0);
        }
        Gizmos.color = Color.red;                             // 색깔 지정        
        Vector3 p0 = destinationArea.position + Vector3.up * MaxY - Vector3.right * 0.5f;
        Vector3 p1 = destinationArea.position + Vector3.up * MaxY + Vector3.right * 0.5f;
        Vector3 p2 = destinationArea.position + Vector3.up * MinY + Vector3.right * 0.5f;
        Vector3 p3 = destinationArea.position + Vector3.up * MinY - Vector3.right * 0.5f;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
    }

    protected override void Spawn()
    {
        Asteroid asteroid = Factory.Instance.GetAsteroid(GetSpawnPosition());
        asteroid.SetDestination(GetDestination());
    }

    Vector3 GetDestination()
    {
        Vector3 pos = destinationArea.position;
        pos.y += Random.Range(MinY, MaxY);  // 현재 위치에서 높이만 (-4 ~ +4) 변경

        return pos;
    }
}
