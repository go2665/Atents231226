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

// ItemData_Food
// - 획득하면 HP가 있는 대상이면 HP증가(틱으로 리젠)
// - 맴버 변수 : 틱당 회복량, 틱인터벌, 틱회수

// ItemData_Drink
//  - 획득하면 MP가 있는 대상이면 MP증가(리젠)
//  - 멤버 변수 : 전체 회복량, 회복시간