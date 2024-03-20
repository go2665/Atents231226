using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_InventoryUI2 : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

#if UNITY_EDITOR
    private void Start()
    {
        // 실습 : Test_InventoryPrint 완성하기
        //[루비(1/10), 사파이어(2/3), (빈칸), 에메랄드(3/5), (빈칸), (빈칸) ]
        inven = new Inventory(null);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Ruby);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Sapphire);
        inven.AddItem(ItemCode.Emerald);
        inven.AddItem(ItemCode.Emerald);
        inven.AddItem(ItemCode.Emerald);
        inven.AddItem(ItemCode.Emerald);
        inven.MoveItem(2, 3);
        inven.AddItem(ItemCode.Sapphire, 2);
        inven.AddItem(ItemCode.Sapphire, 4);
        inven.AddItem(ItemCode.Sapphire, 4);
        inven.AddItem(ItemCode.Sapphire, 5);
        inven.AddItem(ItemCode.Sapphire, 5);
        inven.AddItem(ItemCode.Sapphire, 5);
        inven.Test_InventoryPrint();

        inventoryUI.InitializeInventory(inven);
    }


#endif
}
