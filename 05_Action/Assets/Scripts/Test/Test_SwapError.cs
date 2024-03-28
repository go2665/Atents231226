using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SwapError : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 테스트용 기본 세팅
        Player player = GameManager.Instance.Player;
        Inventory inven = player.Inventory;

        inven.AddItem(ItemCode.SilverSword);
        inven.AddItem(ItemCode.IronSword);
        inven.AddItem(ItemCode.OldSword);

        Debug.Log("아이템 장비 시도");
        inven[0].EquipItem(inven.Owner.gameObject);

        Debug.Log("0 -> temp로 이동");
        inven.MoveItem(0, 9999999);
        Debug.Log("temp -> 1로 이동");
        inven.MoveItem(9999999, 1);

        InventoryUI inventoryUI = FindAnyObjectByType<InventoryUI>();
        inventoryUI.Open();
    }
}
