using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - ManaPotion", menuName = "Scriptable Object/Item Data - ManaPotion", order = 5)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("마나 포션 데이터")]
    public float totalRegen = 50.0f;
    public float duration = 1.0f;

    public bool Use(GameObject target)
    {
        bool result = false;
        IMana mana = target.GetComponent<IMana>();
        if (mana != null)
        {
            // target은 마나가 있다.
            if(mana.MP < mana.MaxMP)
            {
                mana.ManaRegenerate(totalRegen, duration);
                result = true;
            }
        }

        return result;
    }
}