using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : TestBase
{
    public int score = 100;
    public RankLine line;
    public RankPanel panel;

    Player player;
#if UNITY_EDITOR
    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Test_Die();        
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Test_SetScore(score);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        line.SetData("가가가", 1000000);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        panel.Test_DefaultRankPanel();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        panel.Test_SaveRankPanel();

    }
#endif
}
