using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneController : MonoBehaviour
{
    public float cartSpeed = 20.0f;
    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;
    Player player;

    private void Awake()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        cart = GetComponentInChildren<CinemachineDollyCart>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onDie += DeathSceneStart;
    }

    private void DeathSceneStart()
    {
        vCam.Priority = 100;    // 이 가상카메라로 찍기 위해 우선 순위 높이기
        cart.m_Speed = cartSpeed;
    }

    private void Update()
    {
        transform.position = player.transform.position;
    }
}
