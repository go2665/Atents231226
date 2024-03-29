using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 웨이포인트를 저장하고 처리하는 클래스
/// </summary>
public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// 웨이포인트 지점들
    /// </summary>
    Transform[] children;

    /// <summary>
    /// 다음 목적지의 인덱스
    /// </summary>
    int index = 0;

    /// <summary>
    /// 다음 목적지의 위치
    /// </summary>
    public Vector3 NextTarget => children[index].position;

    private void Awake()
    {
        // 자식들을 전부 웨이포인트로 사용
        children = new Transform[transform.childCount];
        for(int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음 웨이포인트 지점을 설정하기 위한 함수
    /// </summary>
    public void StepNextWaypoint()
    {
        index++;
        index %= children.Length;
    }
}