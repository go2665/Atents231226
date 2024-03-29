using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyHitAndAttack : TestBase
{
    public Transform test;

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory[0].EquipItem(player.gameObject);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHitEffect(test.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
    }
}
