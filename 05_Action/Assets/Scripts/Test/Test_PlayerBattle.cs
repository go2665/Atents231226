using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerBattle : TestBase
{
    public float damage = 10;
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory[0].EquipItem(player.gameObject);
        player.Inventory.AddItem(ItemCode.KiteShield);
        player.Inventory[1].EquipItem(player.gameObject);

        player.Inventory.AddItem(ItemCode.IronSword);
        player.Inventory.AddItem(ItemCode.OldSword);
        player.Inventory.AddItem(ItemCode.RoundShield);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.Defence(damage);        
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
    }
}
