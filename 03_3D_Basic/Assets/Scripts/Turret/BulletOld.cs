using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOld : MonoBehaviour
{
    public float initialSpeed = 20.0f;
    public float lifeTime = 10.0f;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = initialSpeed * transform.forward;

        Destroy(this.gameObject, lifeTime);
    }
}
