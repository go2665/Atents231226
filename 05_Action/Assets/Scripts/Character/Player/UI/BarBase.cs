using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    public Color color = Color.white;

    protected Slider slider;
    protected TextMeshProUGUI current;
    protected TextMeshProUGUI max;

    protected float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform child = transform.GetChild(2);
        current = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        max = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(0);
        Image bgImage = child.GetComponent<Image>();
        bgImage.color = new(color.r, color.g, color.b, color.a * 0.5f);
        child = transform.GetChild(1);
        Image fillImage = child.GetComponentInChildren<Image>();
        fillImage.color = color;
    }

    protected void OnValueChange(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        slider.value = ratio;
        current.text = $"{(ratio * maxValue):f0}";
    }
}
