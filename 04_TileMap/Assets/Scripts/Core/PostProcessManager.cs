using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    /// <summary>
    /// 포스트프로세스가 적용되는 볼륨
    /// </summary>
    Volume postProcessVolume;

    /// <summary>
    /// 볼륨안에 있는 비네트용 객체
    /// </summary>
    Vignette vignette;

    public AnimationCurve curve;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);   // 볼륨에서 비네트를 가져오도록 시도
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.onLifeTimeChange += OnLifeTimeChange;
    }

    private void OnLifeTimeChange(float ratio)
    {
        vignette.intensity.value = curve.Evaluate(ratio);
    }
}
