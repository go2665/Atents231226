using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    CinemachineImpulseSource cameraImpulseSource;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

        cameraImpulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }

    public void CameraShake(float force)
    {
        cameraImpulseSource.GenerateImpulseWithVelocity(force * Random.insideUnitCircle.normalized);
    }
}
