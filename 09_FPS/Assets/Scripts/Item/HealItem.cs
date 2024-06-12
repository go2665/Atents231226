using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : ItemBase
{
    public float heal = 20.0f;

    protected override void OnItemConsum(Player player)
    {
        // heal만큼 HP 회복
        player.HP += heal;
    }
}
