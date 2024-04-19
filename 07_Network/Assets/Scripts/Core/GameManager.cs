using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetSingleton<GameManager>
{
    /// <summary>
    /// 로거(텍스트 출력 및 채팅용)
    /// </summary>
    Logger logger;

    /// <summary>
    /// 내 플레이어(접속 안했으면 null)
    /// </summary>
    NetPlayer player;
    public NetPlayer Player => player;

    /// <summary>
    /// 현재 동접자 수
    /// </summary>
    NetworkVariable<int> playersInGame = new NetworkVariable<int>(0);

    /// <summary>
    /// 동접자의 수가 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<int> onPlayersInGameChange;

    /// <summary>
    /// 현재 사용자의 이름
    /// </summary>
    string userName = "디폴트";
    public string UserName
    {
        get => userName;
        set
        {
            userName = value;
            onUserNameChange?.Invoke(userName);
        }
    }
    public Action<string> onUserNameChange;

    /// <summary>
    /// 현재 사용자의 색상
    /// </summary>
    Color userColor = Color.clear;
    public Color UserColor
    {
        get => userColor;
        set
        {
            userColor = value;
            onUserColorChange?.Invoke(userColor);
        }
    }
    public Action<Color> onUserColorChange;

    NetPlayerDecorator deco;


    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();

        // 어떤 클라이언트가 접속/접속해제 했을 때 실행(서버에는 항상 실행, 클라이언트는 자기것만 실행)
        NetworkManager.OnClientConnectedCallback += OnClientConnect;        
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;    // 어떤 클라이언트가 접속해제할 때마다 실행

        playersInGame.OnValueChanged += (_, newValue) => onPlayersInGameChange?.Invoke(newValue);   // 동접자 숫자 변경 알리기
    }

    /// <summary>
    /// 어떤 클라이언트가 접속했을 때 처리를 하는 함수
    /// </summary>
    /// <param name="id">접속한 클라이언트의 id</param>
    private void OnClientConnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        if (netObj.IsOwner)
        {
            player = netObj.GetComponent<NetPlayer>();
            player.gameObject.name = $"Player_{id}";
            
            deco = netObj.GetComponent<NetPlayerDecorator>();
            deco.SetName(UserName);

            foreach (var other in NetworkManager.SpawnManager.SpawnedObjectsList)
            {                
                NetPlayer otherPlayer = other.GetComponent<NetPlayer>();
                if (otherPlayer != null && otherPlayer != player)
                {                    
                    otherPlayer.gameObject.name = $"OtherPlayer_{other.OwnerClientId}";
                }
            }
        }
        else
        {
            NetPlayer other = netObj.GetComponent<NetPlayer>();
            if(other != null && other != player)
            {
                netObj.gameObject.name = $"OtherPlayer_{id}";
            }
        }

        if(IsServer)
        {
            playersInGame.Value++;  // 서버에서만 증가
        }
    }

    /// <summary>
    /// 어떤 클라이언트가 접속 해제했을 때 처리를 하는 함수
    /// </summary>
    /// <param name="id">접속 해제한 클라이언트의 id</param>
    private void OnClientDisconnect(ulong id)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        if (netObj.IsOwner)
        {
            player = null;
        }

        if (IsServer)
        {
            playersInGame.Value--;  // 서버에서만 감소
        }
    }


    /// <summary>
    /// 로거에 문자열을 추가하는 함수
    /// </summary>
    /// <param name="message">추가할 문자열</param>
    public void Log(string message)
    {
        logger.Log(message);
    }
}

// 실습
// 현재 접속해있는 플레이어의 수를 UI로 표시하기