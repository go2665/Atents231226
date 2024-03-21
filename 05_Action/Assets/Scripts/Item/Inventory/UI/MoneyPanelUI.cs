using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanelUI : MonoBehaviour
{
    TextMeshProUGUI moneyText;

    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Refresh(int money)
    {
        moneyText.text = money.ToString("N0");
    }
}
