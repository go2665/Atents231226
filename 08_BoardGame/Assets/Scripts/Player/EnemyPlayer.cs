using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : PlayerBase
{
    protected override void Start()
    {
        base.Start();

        opponent = gameManager.UserPlayer;
    }
}
