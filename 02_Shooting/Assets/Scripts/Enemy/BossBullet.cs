using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : RecycleObject
{
    // 시작하자마자 계속 오른쪽으로 초속 7로 움직이게 만들기

    /// <summary>
    /// 총알의 이동 속도
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// 총알의 수명
    /// </summary>
    public float lifeTime = 10.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LifeOver(lifeTime));
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Factory.Instance.GetHitEffect(collision.contacts[0].point);

            gameObject.SetActive(false);    // 비활성화 -> 풀로 되돌리기
        }
    }
}
