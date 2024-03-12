using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inventory : TestBase
{
    // Start is called before the first frame update
    void Start()
    {
        ItemData data = GameManager.Instance.ItemData[ItemCode.Ruby];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
