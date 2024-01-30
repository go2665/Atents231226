using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretTrace : MonoBehaviour
{
    /// <summary>
    /// 시야 범위
    /// </summary>
    public float sightRange = 10.0f;

    /// <summary>
    /// 터렛의 머리가 돌아가는 속도
    /// </summary>
    public float turnSpeed = 2.0f;

    /// <summary>
    /// 터렛이 총알을 발사를 시작하는 발사각
    /// </summary>
    public float fireAngle = 10.0f;

    private void OnDrawGizmos()
    {
        //Gizmos
        //Handles.DrawWireDisc
    }
}

// 실습 : 추적용 터렛 만들기
// 1. 플레이어가 터렛으로 부터 일정 거리안에 있으면 플레이어쪽으로 BarrelBody가 회전한다(플레이어를 바라보기, y축으로만 회전)
// 2. 플레이어가 터렛의 발사각 안에 있으면 총알을 주기적으로 발사한다.
// 3. 플레이어가 터렛의 발사각 안에 없으면 총알 발사를 정지한다.
// 4. Gizmos를 통해 시야 범위와 발사각을 그린다.(Handles 추천)