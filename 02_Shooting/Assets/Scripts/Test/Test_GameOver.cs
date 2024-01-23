using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : TestBase
{
    public int score = 100;

    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Test_AddScore(score);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Test_AddScore(-score);
        Debug.Log($"Score : {player.Score}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.Test_Die();
    }
}
