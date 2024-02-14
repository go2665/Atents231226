using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class VirtualButton : MonoBehaviour, IPointerClickHandler
{
    Image coolDown;

    public Action onClick;

    void Awake()
    {
        Transform child = transform.GetChild(1);
        coolDown = child.GetComponent<Image>();
        coolDown.fillAmount = 0.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void RefreshCoolTime(float ratio)
    {
        coolDown.fillAmount = ratio;
    }

    public void Stop()
    {
        onClick = null;
        coolDown.fillAmount = 1.0f;
    }
}
