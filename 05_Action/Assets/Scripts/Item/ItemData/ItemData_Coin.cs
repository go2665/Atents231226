using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Coin", menuName = "Scriptable Object/Item Data - Coin", order = 1)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            // target은 Player다.
            player.Money += (int)price; // 동전 아이템의 가격만큼 플레이어의 돈 증가

        }
    }
}