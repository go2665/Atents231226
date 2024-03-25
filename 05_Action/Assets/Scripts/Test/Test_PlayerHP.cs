using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerHP : TestBase
{
    Player player;
    public float data = 10.0f;

    public ItemCode code = ItemCode.Ruby;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 플레이어 MP 증가
        player.HP += data;
        player.MP += data;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 플레이어 MP 감소
        player.HP -= data;
        player.MP -= data;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 플레이어 MP 재생
        player.HealthRegenerate(data, 1);
        player.ManaRegenerate(data, 1);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // 플레이어 MP 틱당 재생
        player.HealthRegenerateByTick(3, 0.5f, 4);
        player.ManaRegenerateByTick(3, 0.5f, 4);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItem(code);
    }
}
