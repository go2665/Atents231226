using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if(player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }
    WorldManager worldManager;
    public WorldManager World => worldManager;

    /// <summary>
    /// 슬라임의 이동 경로를 게임에서 보이게 할지 말지를 결정하는 변수(true면 보이고, false면 안보인다.)
    /// </summary>
    public bool showSlimePath = false;

    private void OnValidate()
    {
        Slime[] slimes = FindObjectsOfType<Slime>(true);    // 비활성화 된 오브젝트 포함해서 모두 찾기
        foreach(Slime slime in slimes)
        {
            slime.ShowPath(showSlimePath);                  // 경로 표시 여부 설정
        }
    }

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        worldManager = GetComponent<WorldManager>();
        worldManager.PreInitialize();

    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        worldManager.Initialize();
    }

}
