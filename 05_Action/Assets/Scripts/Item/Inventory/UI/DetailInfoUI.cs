using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DetailInfoUI : MonoBehaviour
{
    Image icon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI price;
    TextMeshProUGUI description;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 일시 정지 모드(true면 일시 정지, false면 사용 중)
    /// </summary>
    bool isPause = false;

    /// <summary>
    /// 일시 정지 모드를 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsPause
    {
        get => isPause;
        set
        {
            isPause = value;
            if(isPause)
            {
                Close();    // 일시 정지가 되면 열려있던 상세 정보창도 닫는다.
            }
        }
    }

    /// <summary>
    /// 알파값이 변하는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();  // 컴포넌트 찾기
        canvasGroup.alpha = 0.0f;

        Transform child = transform.GetChild(0);
        icon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        price = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        description = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 상세 정보창 여는 함수
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    public void Open(ItemData itemData)
    {
        if(!IsPause && itemData != null)
        {
            // 컴포넌트들 채우기
            icon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            price.text = itemData.price.ToString("N0");
            description.text = itemData.itemDescription;

            canvasGroup.alpha = 0.0001f; // MovePosition이 alpha가 0보다 클때만 실행되니 미리 조금만 올리기
            MovePosition(Mouse.current.position.ReadValue()); // 보이기 전에 커서 위치와 상세 정보창 옮기기

            // 알파 변경 시작(0->1)
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// 상세 정보창이 닫힐 때 실행되는 함수
    /// </summary>
    public void Close()
    {
        // 알파 변경 시작(1->0)
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    /// <summary>
    /// 상세 정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">스크린 좌표</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // 화면의 가로 해상도

        if( canvasGroup.alpha > 0.0f )  // 보이는 상황인지 확인
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // 얼마나 넘쳤는지 확인            
            screenPos.x -= Mathf.Max(0, over);  // over를 양수로만 사용(음수일때는 별도 처리 필요없음)
            rect.position = screenPos;
        }
    }

    /// <summary>
    /// 알파를 0 -> 1로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    /// <summary>
    /// 알파를 1 -> 0으로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha += -Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
    }

}
