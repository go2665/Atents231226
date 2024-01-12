using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlanet : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float minRightEnd = 30.0f;
    public float maxRightEnd = 60.0f;
    public float minY = -8.0f;
    public float maxY = -5.0f;

    float baseLineX;

    private void Start()
    {
        baseLineX = transform.position.x;   // 기준선은 초기 위치(x)
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * -transform.right); // 왼쪽으로 이동하다가

        if(transform.position.x < baseLineX)    // 기준선보다 왼쪽으로 가면
        {
            transform.position = new Vector3(
                Random.Range(minRightEnd, maxRightEnd), // 오른쪽으로 랜덤한 거리만큼 이동
                Random.Range(minY, maxY),               // 위 아래도 랜덤으로 조절
                0.0f);
        }
    }
}
