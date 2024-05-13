using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPrinter : MonoBehaviour
{
    TextMeshProUGUI turn;

    private void Awake()
    {
        turn = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        turn.text = "1 턴";
        GameManager.Instance.TurnController.onTurnStart += OnTurnStart;
    }

    private void OnTurnStart(int number)
    {
        turn.text = $"{number} 턴";
    }
}
