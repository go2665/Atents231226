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

    NetPlayer player;
    public NetPlayer Player => player;


    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();

        // 어떤 클라이언트가 접속/접속해제 했을 때 실행(서버에는 항상 실행, 클라이언트는 자기것만 실행)
        NetworkManager.OnClientConnectedCallback += OnClientConnect;        
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;    // 어떤 클라이언트가 접속해제할 때마다 실행
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

            foreach(var other in NetworkManager.SpawnManager.SpawnedObjectsList)
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
