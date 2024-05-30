
using System;
using UnityEngine;
using UnityEngine.VFX;

public class BulletHole : RecycleObject
{
    VisualEffect effect;
    float duration;

    readonly int OnStartEventID = Shader.PropertyToID("OnStart");
    readonly int DurationID = Shader.PropertyToID("Duration");
    readonly int DurationRangeID = Shader.PropertyToID("DurationRange");
    readonly int DebrisSpawnReflectID = Shader.PropertyToID("DebrisSpawnReflect");


    private void Awake()
    {
        effect = GetComponent<VisualEffect>();
        float durationRange = effect.GetFloat(DurationRangeID);
        duration = effect.GetFloat(DurationID) + durationRange;  // 원래 duration +- durationRange가 실제 범위니까
    }

    /// <summary>
    /// 총알 구멍 이팩트 초기화용 함수
    /// </summary>
    /// <param name="position">생성될 위치</param>
    /// <param name="normal">생성될 면의 노멀</param>
    /// <param name="reflect">총알 반사 방향</param>
    public void Initialize(Vector3 position, Vector3 normal, Vector3 reflect)
    {
        transform.position = position;      // 이팩트 위치 설정
        transform.forward = -normal;        // 이팩트 회전 설정

        effect.SetVector3(DebrisSpawnReflectID, reflect);   // 파편 반사 방향 설정

        effect.SendEvent(OnStartEventID);   // 이팩트 재생 시작
        StartCoroutine(LifeOver(duration)); // 충분히 재생되었으면 다시 풀로 되돌리기
    }
}
