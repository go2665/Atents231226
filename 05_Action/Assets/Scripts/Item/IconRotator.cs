using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotator : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;

    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    float timeElapsed = 0.0f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);   // 초기 랜덤 설정
        transform.position = transform.parent.position + Vector3.up * maxHeight;
    }

    private void Update()
    {
        // 위아래로 계속 움직임(높이 : minHeight ~ maxHeight), 삼각함수 활용
        timeElapsed += Time.deltaTime * moveSpeed;

        // cos() + 1 = 0 ~ 2
        // (cos() + 1) * 0.5 = 0 ~ 1
        // ((cos() + 1) * 0.5) * (max - min) = 0 ~ (max-min)

        // min + ((cos() + 1) * 0.5) = min ~ (min+1)

        // min + ((cos() + 1) * 0.5) * (max - min) = min ~ max

        Vector3 pos;
        pos.x = transform.parent.position.x;
        pos.y = minHeight + ((Mathf.Cos(timeElapsed) + 1) * 0.5f) * (maxHeight - minHeight);
        pos.z = transform.parent.position.z;
        transform.position = pos;


        // y축 기준으로 계속 회전
        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }
}
