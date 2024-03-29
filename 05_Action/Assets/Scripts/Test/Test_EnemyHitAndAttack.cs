using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_EnemyHitAndAttack : TestBase
{
    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.Inventory.AddItem(ItemCode.SilverSword);
        player.Inventory[0].EquipItem(player.gameObject);
    }
}
