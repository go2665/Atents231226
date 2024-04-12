using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    /// <summary>
    /// 인풋 필드의 텍스트를 설정하는 함수
    /// </summary>
    /// <param name="name">설정될 텍스트</param>
    public void SetPlayerName(string name)
    {
        inputField.text = name;
    }

    /// <summary>
    /// 인풋 필드에 입력되어 있는 텍스트를 가져오는 함수
    /// </summary>
    /// <returns></returns>
    public string GetPlayerName()
    {
        return inputField.text;
    }
}
