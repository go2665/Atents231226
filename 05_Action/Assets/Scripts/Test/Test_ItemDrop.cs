using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemDrop : TestBase
{
    public InventoryUI inventoryUI;
    Inventory inven;

    public ItemCode code = ItemCode.Ruby;

    public Transform target;
    public bool useNoise = false;

#if UNITY_EDITOR

    private void Start()
    {
        target = transform.GetChild(0);

        inven = new Inventory(GameManager.Instance.Player);
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
        inven.AddItem(code);
        inven.Test_InventoryPrint();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItem(code);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.MakeItem(code, target.position, useNoise);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        ItemObject[] items = FindObjectsByType<ItemObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (ItemObject item in items)
        {
            ItemData data = item.ItemData;
            item.End();
            Debug.Log($"{data.itemName} 획득");
        }
    }
#endif
}
