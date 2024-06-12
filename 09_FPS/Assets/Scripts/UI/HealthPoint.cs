using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{
    TextMeshProUGUI hp;

    private void Awake()
    {
        hp = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.Player.onHPChange += OnHealthChange;
    }

    private void OnHealthChange(float health)
    {
        hp.text = health.ToString("f0");
    }
}
