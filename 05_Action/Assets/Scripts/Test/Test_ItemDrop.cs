using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

    public ItemCode code = ItemCode.Ruby;

    [Range(0, 5)]
    public uint fromIndex = 0;

    [Range(0, 5)]
    public uint toIndex = 0;

    public ItemSortBy sortBy = ItemSortBy.Code;
    public bool isAcending = true;

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

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 추가
        inven.AddItem(code, fromIndex);
        inven.Test_InventoryPrint();
    }


#endif
}
