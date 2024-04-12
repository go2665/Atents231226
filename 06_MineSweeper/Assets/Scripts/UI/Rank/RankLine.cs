using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI rankText;
    TextMeshProUGUI record;
    TextMeshProUGUI recordText;
    TextMeshProUGUI rankerName;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        rank = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        rankText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        record = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        recordText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        rankerName = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// RankLine에 데이터를 세팅해서 보이게 만드는 함수
    /// </summary>
    /// <typeparam name="T">기록에 대한 데이터 타입(int,float 목적)</typeparam>
    /// <param name="rankData">등수</param>
    /// <param name="recordData">기록</param>
    /// <param name="nameData">기록을 달성한 사람</param>
    public void SetData<T>(int rankData, T recordData, string nameData)
    {
        rank.text = rankData.ToString();

        if(recordData.GetType() == typeof(float))
        {
            record.text = $"{recordData:f1}";       // float은 소수점 첫째자리까지만
        }
        else
        {
            record.text = recordData.ToString();    // 다른 데이터들은 그냥 넣기
        }

        rankerName.text = nameData;

        // 데이터 안들어가는 것들 켜기
        rankText.enabled = true;
        recordText.enabled = true;
    }

    /// <summary>
    /// RankLine을 안보이게 비우는 함수
    /// </summary>
    public void ClearLine()
    {
        rank.text = string.Empty;
        record.text = string.Empty;
        rankerName.text = string.Empty;

        // 데이터 안들어가는 것들 끄기
        rankText.enabled = false;
        recordText.enabled = false;
    }
}
