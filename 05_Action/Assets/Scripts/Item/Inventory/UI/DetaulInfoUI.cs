using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetaulInfoUI : MonoBehaviour
{
    Image icon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI price;
    TextMeshProUGUI description;

    CanvasGroup canvasGroup;

    public float alphaChangeSpeed = 10.0f;

    public void Open(ItemData itemData)
    {
        // 컴포넌트들 채우기
        // 알파 변경 시작(0->1)
    }

    public void Close()
    {
        // 알파 변경 시작(1->0)
    }

    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // 화면의 가로 해상도

        // 디테일 인포창을 screenPos로 이동시킨다.
        // 단 디테일 인포창이 화면 밖으로 벗어날 경우라도 창 전체가 보여야 한다.
    }
}
