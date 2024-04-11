using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    TMP_InputField inputField;
    
    string playerName;

    public string PlayerName
    {
        get => playerName;
    }

    public Action<string> onPlayerNameSet;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener((text) =>
        {
            playerName = text;
            onPlayerNameSet?.Invoke(playerName);
        });
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        inputField.text = name;
    }
}
