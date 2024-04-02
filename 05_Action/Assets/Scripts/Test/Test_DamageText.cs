using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_DamageText : TestBase
{
    Transform test;
    public float damage = 10;
    Player player;

    private void Start()
    {
        test = transform.GetChild(0);

        player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory[0].EquipItem(player.gameObject);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDamageText(10, test.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Defence(damage);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }
}
