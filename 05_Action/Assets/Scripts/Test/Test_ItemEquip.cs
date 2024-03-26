using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemEquip : TestBase
{
    private void Start()
    {
        Factory.Instance.MakeItem(ItemCode.IronSword);
        Factory.Instance.MakeItem(ItemCode.IronSword);
        Factory.Instance.MakeItem(ItemCode.SilverSword);
        Factory.Instance.MakeItem(ItemCode.OldSword);        

    }
}
