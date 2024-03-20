using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotator : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float moveSpeed = 2.0f;

    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);   // 초기 랜덤 설정
    }

    private void Update()
    {
        
    }
}
