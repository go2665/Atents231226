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
        Factory.Instance.GetPlayerHitEffect(test.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.HP = -10000;

    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.Test_DropItems(1000000);
    }
}
