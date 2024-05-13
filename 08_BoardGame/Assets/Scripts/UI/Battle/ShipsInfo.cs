using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipsInfo : MonoBehaviour
{
    public PlayerBase player;
    TextMeshProUGUI[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Ship[] ships = player.Ships;
        for(int i = 0; i < ships.Length; i++)
        {
            PrintHP(texts[i], ships[i]);

            int index = i;
            ships[i].onHit += (ship) => PrintHP(texts[index], ship);
            ships[i].onSink += (_) =>
            {
                texts[index].fontSize = 40.0f;
                texts[index].text = "<#ff0000>Destroy!!</color>";
            };
        }
    }

    void PrintHP(TextMeshProUGUI text, Ship ship)
    {
        text.text = $"{ship.HP}/{ship.Size}";
    }
}

// 실습
// 1. 함선의 HP를 "현재HP/최대HP" 형식으로 출력하기
// 2. 함선이 침몰하면 빨간색으로 "Destroy!!"로 출력하기
// 3. Enemy용 ShipInfo UI도 추가하기