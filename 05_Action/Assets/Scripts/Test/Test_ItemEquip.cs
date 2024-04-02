using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ItemEquip : TestBase
{
    private void Start()
    {
        Factory.Instance.MakeItem(ItemCode.IronSword);
        Factory.Instance.MakeItem(ItemCode.IronSword);
        Factory.Instance.MakeItem(ItemCode.SilverSword);
        Factory.Instance.MakeItem(ItemCode.OldSword);        
        Factory.Instance.MakeItem(ItemCode.KiteShield);        
        Factory.Instance.MakeItem(ItemCode.RoundShield);        

    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }
}
