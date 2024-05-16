using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultAnalysis : MonoBehaviour
{
    public int TotalAttackCount
    {
        set
        {
            // Value의 첫번째 자식 텍스트 수정
            texts[0].text = $"<b>{value}</b> 회";
        }
    }

    public int SuccessAttackCount
    {
        set
        {
            // Value의 두번째 자식 텍스트 수정
            texts[1].text = $"<b>{value}</b> 회";
        }
    }

    public int FailAttackCount
    {
        set
        {
            // Value의 세번째 자식 텍스트 수정
            texts[2].text = $"<b>{value}</b> 회";
        }
    }

    public float SuccessAttackRate
    {
        set
        {
            // Value의 네번째 자식 텍스트 수정
            // 소수점 첫째자리까지만 출력
            texts[3].text = $"<b>{(value * 100.0f):f1}</b> %";
        }
    }

    TextMeshProUGUI[] texts;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        texts = child.GetComponentsInChildren<TextMeshProUGUI>();
    }

}
