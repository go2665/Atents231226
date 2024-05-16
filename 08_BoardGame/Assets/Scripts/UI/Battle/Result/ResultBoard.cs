using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultBoard : MonoBehaviour
{
    public Material victory;
    public Material defeat;
    TextMeshProUGUI result;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        result = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 열고 닫기용 토글버튼
    /// </summary>
    public void Toggle()
    {
        // 실행될 때마다 번갈아가면서 Board가 열리거나 닫힌다.
        gameObject.SetActive(!gameObject.activeSelf);
    }

    /// <summary>
    /// 승리/패배 글자와 머티리얼을 설정하는 함수
    /// </summary>
    /// <param name="isVictory">true면 승리, false면 패배</param>
    public void SetVictoryDefeat(bool isVictory)
    {
        if (isVictory)
        {
            result.fontMaterial = victory;
            result.text = "승리!";
        }
        else
        {
            result.fontMaterial = defeat;
            result.text = "패배...";
        }
    }
}
