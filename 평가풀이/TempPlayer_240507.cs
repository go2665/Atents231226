using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempPlayer_240507 : MonoBehaviour
{
    public float health = 50;    
    public float maxHealth = 100;

    TextMeshProUGUI healthText = null;
    Slider healthSlider = null;
    Button healthUp = null;
    Button healthDown = null;

    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            healthSlider.value = health / maxHealth;
            healthText.text = $"{health:f0}/{maxHealth:f0}";
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {        
        Canvas canvas = FindAnyObjectByType<Canvas>();
        Transform child = canvas.transform.GetChild(0);
        healthSlider = child.GetComponent<Slider>();

        child = canvas.transform.GetChild(1);
        healthText = child.GetComponent<TextMeshProUGUI>();

        child = canvas.transform.GetChild(2);
        healthUp = child.GetComponent<Button>();
        healthUp.onClick.AddListener(UpClick);
        Text up = healthUp.GetComponentInChildren<Text>();
        up.text = "Up";

        child = canvas.transform.GetChild(3);
        healthDown = child.GetComponent<Button>();
        healthDown.onClick.AddListener(DownClick);
        Text down = healthDown.GetComponentInChildren<Text>();
        down.text = "Down";

        Health = health;
    }

    private void DownClick()
    {
        Health -= maxHealth * 0.1f;
    }

    private void UpClick()
    {
        Health += maxHealth * 0.1f;
    }
}
