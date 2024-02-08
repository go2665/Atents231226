using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    public float duration = 5.0f;
    ParticleSystem ps;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        ps = child.GetComponent<ParticleSystem>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        ps.Play();      // 이팩트 재생
        IAlive live = target.GetComponent<IAlive>();
        if(live != null)
        {
            live.Die(); // 죽일 수 있는 대상은 죽이기
        }

        StopAllCoroutines();            // 이전 코루틴 정지
        StartCoroutine(StopEffect());   // 5초뒤에 이팩트를 정지시키는 코루틴 실행
    }

    IEnumerator StopEffect()
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
    }
}
