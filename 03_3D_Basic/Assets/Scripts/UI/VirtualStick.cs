using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class VirtualStick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    RectTransform handleRect;
    RectTransform containerRect;

    float stickRange;

    public Action<Vector2> onMoveInput;

    void Awake()
    {
        containerRect = GetComponent<RectTransform>();

        Transform child = transform.GetChild(0);
        handleRect = child.GetComponent<RectTransform>();
        //handleRect = child as RectTransform;

        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // eventData.position : 마우스 포인터의 현재 스크린 좌표
        //Debug.Log($"드래그 중 : {eventData.position}");

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect,                  // 이 영역안에서
            eventData.position,             // 이 스크린 좌표가
            eventData.pressEventCamera,     // (기준이 되는 카메라)
            out Vector2 position            // 로컬로 얼마나 움직인 건지 리턴
            );

        position = Vector2.ClampMagnitude(position, stickRange);

        InputUpdate(position);
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    //if(eventData.button == PointerEventData.InputButton.Left)   // 왼쪽 버튼이 떨어졌을 때
    //    InputUpdate(Vector2.zero);
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    // IPointerUpHandler와 IPointerDownHandler는 항상 쌍으로 있어야 한다.
    //}

    private void InputUpdate(Vector2 inputDelta)
    {
        handleRect.anchoredPosition = inputDelta;
        onMoveInput?.Invoke(inputDelta/stickRange); // 크기를 1로 변환해서 보냄
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // IBeginDragHandler와 IEndDragHandler를 사용하려면 IDragHandler 필수
        InputUpdate(Vector2.zero);
    }

    public void Stop()
    {
        onMoveInput = null;
    }
}
