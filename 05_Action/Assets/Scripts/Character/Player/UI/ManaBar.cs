using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : BarBase
{
    private void Start()
    {
        Player player = GameManager.Instance.Player;
        if (player != null)
        {
            maxValue = player.MaxMP;
            max.text = $" / {maxValue}";
            current.text = player.MP.ToString("f0");
            slider.value = player.MP / maxValue;
            player.onManaChange += OnValueChange;
        }
    }
}
