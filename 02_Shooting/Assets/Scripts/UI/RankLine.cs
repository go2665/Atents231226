using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        nameText = child.GetComponent<TextMeshProUGUI>();
        
        child = transform.GetChild(2);
        scoreText = child.GetComponent<TextMeshProUGUI>();
    }

    public void SetData(string rankerName, int score)
    {
        nameText.text = rankerName;
        scoreText.text = score.ToString("N0");  // 숫자 3자리마다 콤마찍기
    }
}
