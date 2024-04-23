using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class NetEnergyOrb : MonoBehaviour
{
    /// <summary>
    /// 발사 초기 속도
    /// </summary>
    public float speed = 10.0f;

    /// <summary>
    /// 수명
    /// </summary>
    public float lifeTime = 20.0f;

    /// <summary>
    /// 폭발 범위
    /// </summary>
    public float expolsionRadius = 5.0f;


    Rigidbody rigid;
    VisualEffect effect;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        effect = GetComponent<VisualEffect>();
    }

    private void Start()
    {
        transform.Rotate(-30.0f, 0, 0);
        rigid.velocity = speed * transform.forward;
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] result = Physics.OverlapSphere(transform.position, expolsionRadius, LayerMask.GetMask("Player"));

        if(result.Length > 0)
        {
            foreach(Collider col in result)
            {
                Debug.Log(col.gameObject.name);
            }
        }

        StartCoroutine(EffectFinishProcess());
    }

    IEnumerator EffectFinishProcess()
    {
        yield return null;

        // 0.5초 동안 baseSize를 expolsionRadius까지 확대
        // 1초동안 baseSize가 0이 될때까기 축소
        // 파티클 생성 중지
        // 파티클 개수가 0이되면 게임 오브젝트 제거

        //effect.SetFloat("Size", );            // 변수 변경하기(타입에 맞게 해야 함)
        //effect.SendEvent("OnEffectFinish");   // 이벤트 날리기
        //effect.aliveParticleCount             // 남아있는 파티클 개수

    }
}
