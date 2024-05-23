using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera FollowCamera => followCamera;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null)
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }
    }
}
