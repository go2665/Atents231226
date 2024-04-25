using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.VFX;

public class NetEnergyOrb : NetworkBehaviour
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


    bool isUsed = false;

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
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            StartCoroutine(SelfDespawn());
        }
    }

    IEnumerator SelfDespawn()
    {
        yield return new WaitForSeconds(lifeTime);
        if(IsServer)
        {
            this.NetworkObject.Despawn();
        }
        else
        {
            RequestDespawnServerRpc();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner && !this.NetworkObject.IsSpawned)  // 오너가 아니면 무시. spawn되기 전에 일어난 충돌은 무시
            return;

        if(!isUsed)
        {
            Collider[] result = Physics.OverlapSphere(transform.position, expolsionRadius, LayerMask.GetMask("Player"));

            if(result.Length > 0)
            {
                foreach(Collider col in result)
                {
                    Debug.Log(col.gameObject.name);
                }
            }

            EffectProcessClientRpc();            
        }
    }

    /// <summary>
    /// ClientRpc : 서버가 모든 클라이언트에게 로컬에서 실행하라고 하는 함수
    /// </summary>
    [ClientRpc]
    void EffectProcessClientRpc()
    {
        if(IsOwner)
        { 
            rigid.useGravity = false;
            rigid.isKinematic = true;
            StartCoroutine(EffectFinishProcess());
            isUsed = true;
        }
    }

    IEnumerator EffectFinishProcess()
    {
        int BaseSize_ID = Shader.PropertyToID("BaseSize");
        int EffectFinishEvent_ID = Shader.PropertyToID("OnEffectFinish");

        float elapsedTime = 0.0f;

        // 0.5초 동안 baseSize를 expolsionRadius까지 확대
        float expendDuration = 0.5f;
        float preCompute = (1 / expendDuration) * expolsionRadius;
        // 첫번째 식
        //      elapsedTime / expendDuration * expolsionRadius;
        // 나누기 하기 싫어서 수정
        //      float pre = 1 / expendDuration;
        //      elapsedTime * pre * expolsionRadius;
        // 곱하기 회수 줄이기
        //      float preCompute = 1 / expendDuration * expolsionRadius;
        //      elapsedTime * preCompute;
        while (elapsedTime < expendDuration)
        {
            elapsedTime += Time.deltaTime;
            effect.SetFloat(BaseSize_ID, elapsedTime * preCompute);
            yield return null;
        }

        // 1초동안 baseSize가 0이 될때까기 축소
        float reductionDuration = 1.0f;
        elapsedTime = reductionDuration;
        float preCompute2 = 1 / reductionDuration;
        while (elapsedTime > 0.0f)
        {
            elapsedTime -= Time.deltaTime;
            effect.SetFloat(BaseSize_ID, elapsedTime * preCompute2 * expolsionRadius);
            yield return null;
        }

        // 파티클 생성 중지
        effect.SendEvent(EffectFinishEvent_ID);
                
        // 파티클 개수가 0이되면 게임 오브젝트 제거
        while(effect.aliveParticleCount > 0)
        {
            yield return null;
        }

        // 모든 파티클 제거
        if(IsServer)
        {
            NetworkObject.Despawn();
        }
        else
        {
            RequestDespawnServerRpc();
        }
    }

    [ServerRpc]
    void RequestDespawnServerRpc()
    {
        this.NetworkObject.Despawn();
    }
}
