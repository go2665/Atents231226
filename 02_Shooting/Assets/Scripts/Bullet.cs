using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 시작하자마자 계속 오른쪽으로 초속 7로 움직이게 만들기

    public float moveSpeed = 7.0f;

    public GameObject effectPrefab;

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right);    // 총 곱한 수는? 3번
        //transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);    // 총 곱한 수는? 4번
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 이제 부딪친 쪽에서 처리
        //if(collision.gameObject.CompareTag("Enemy"))    // 부딪친 상대가 Enemy 태그를 가지고 있으면 삭제
        //{
        //    Destroy(collision.gameObject);
        //}

        Instantiate(effectPrefab, transform.position, Quaternion.identity); // hit 이팩트 생성
        Destroy(gameObject);    // 자기 자신은 무조건 삭제
    }
}

// 실습
// 1. bullet 프리팹에 필요한 컴포넌트 추가하고 설정하기
// 2. 총알은 다른 오브젝트와 부딪치면 자기 자신을 삭제한다.
// 3. 총알은 "Enemy" 태그를 가진 오브젝트와 부딪치면 부딪친 대상을 삭제한다.

// 4. Hit 스프라이트를 이용해 HitEffect라는 프리팹 만들기
// 5. 총알이 부딪친 위치에 HitEffect 생성하기
// 6. HitEffect는 한번만 재생된 후 사라진다.
