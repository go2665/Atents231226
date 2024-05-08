using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 사용자 플레이어(왼쪽)
    /// </summary>
    UserPlayer user;
    public UserPlayer UserPlayer => user;

    /// <summary>
    /// 적 플레이어(오른쪽)
    /// </summary>
    EnemyPlayer enemy;
    public EnemyPlayer EnemyPlayer => enemy;

    /// <summary>
    /// 카메라 진동 소스
    /// </summary>
    CinemachineImpulseSource cameraImpulseSource;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    public TurnManager TurnManager => turnManager;

    /// <summary>
    /// 테스트 모드인지 표시용
    /// </summary>
    public bool IsTestMode = false;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        cameraImpulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();

        turnManager = GetComponent<TurnManager>();
        turnManager.OnInitialize(user, enemy);
    }

    public void CameraShake(float force)
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * Random.insideUnitCircle.normalized);
    }
}
