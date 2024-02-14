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


    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        stick = FindAnyObjectByType<VirtualStick>();
    }
}
