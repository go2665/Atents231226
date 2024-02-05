using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어를 천천히 따라가는 카메라
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// 카메라가 따라다니는 대상
    /// </summary>
    public Transform target;

    /// <summary>
    /// 카메라가 따라다니는 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 플레이어와 카메라의 간격
    /// </summary>
    Vector3 offset;

    /// <summary>
    /// 플레이어와 카메라 간의 거리
    /// </summary>
    float length;

    private void Start()
    {
        if(target == null)
        {
            target = GameManager.Instance.Player.transform.GetChild(7);
        }

        offset = transform.position - target.position;  // target에서 카메라로 가는 방향 벡터
        length = offset.magnitude;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp( transform.position,
            target.position + Quaternion.LookRotation(target.forward) * offset,
            Time.fixedDeltaTime * speed);   // 천천히 따라가는 느낌으로 카메라 이동시키기
        transform.LookAt(target);           // 항상 target을 바라보기


        // 플레이어와 카메라 사이에 장애물이 있으면 충돌지점에 카메라를 이동시킨다.
        Ray ray = new Ray(target.position, transform.position - target.position);
        if( Physics.Raycast(ray, out RaycastHit hitInfo, length) )
        {
            transform.position = hitInfo.point;
        }    

    }
}
