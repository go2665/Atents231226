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

    public void SetPlayerName(string name)
    {
        inputField.text = name;
    }

    public string GetPlayerName()
    {
        return inputField.text;
    }
}
