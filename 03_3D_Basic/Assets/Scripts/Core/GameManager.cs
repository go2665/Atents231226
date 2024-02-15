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

    VirtualStick stick;
    public VirtualStick Stick
    {
        get
        {
            if(stick == null)
                stick = FindAnyObjectByType<VirtualStick>();
            return stick;
        }
    }

    VirtualButton jumpButton;
    public VirtualButton JumpButton
    {
        get
        {
            if (jumpButton == null)
                jumpButton = FindAnyObjectByType<VirtualButton>();
            return jumpButton;
        }
    }


    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        stick = FindAnyObjectByType<VirtualStick>();
        jumpButton = FindAnyObjectByType<VirtualButton>();
    }

    bool isClear = false;

    public Action onGameClear;

    public void GameClear()
    {
        if(!isClear)
        {
            onGameClear?.Invoke();
            isClear = true;
            Debug.Log("게임 클리어");
        }
    }

    bool isOver = false;
    public Action onGameOver;
    public void GameOver()
    {
        if (!isOver)
        {
            onGameOver?.Invoke();
            isOver = true;
            Debug.Log("게임 오버");
        }
    }

    /// <summary>
    /// 플레이 도중임을 확인하는 프로퍼티(게임이 클리어 되지 않았고 게임이 오버되지도 않았다)
    /// </summary>
    public bool IsPlaying => !isClear && !isOver;
}
