using Cinemachine;
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
    string userName = DefaultName;
    
    /// <summary>
    /// 현재 사용자 이름을 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    public string UserName
    {
        get => userName;
        set
        {
            userName = value;
            onUserNameChange?.Invoke(userName); // 변경되었음을 알림
        }
    }

    /// <summary>
    /// 사용자 이름의 변경을 알리기 위한 델리게이트
    /// </summary>
    public Action<string> onUserNameChange;

    /// <summary>
    /// 플레이어의 기본 이름
    /// </summary>
    const string DefaultName = "플레이어";

    /// <summary>
    /// 현재 사용자의 색상
    /// </summary>
    Color userColor = Color.clear;

    /// <summary>
    /// 사용자의 색상을 변경하기 위한 프로퍼티
    /// </summary>
    public Color UserColor
    {
        get => userColor;
        set
        {
            userColor = value;
            onUserColorChange?.Invoke(userColor);   // 변경되었음을 알림
        }
    }

    /// <summary>
    /// 색상이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<Color> onUserColorChange;

    /// <summary>
    /// 플레이어의 이름과 색상을 컨트롤하기 위한 컴포넌트
    /// </summary>
    NetPlayerDecorator deco;

    /// <summary>
    /// 플레이어의 이름과 색상을 컨트롤하는 컴포넌트 확인용 프로퍼티
    /// </summary>
    public NetPlayerDecorator PlayerDeco => deco;

    /// <summary>
    /// 플레이어(자기 자신)가 연결이 해제되었음을 알리는 델리게이트
    /// </summary>
    public Action onPlayerDisconnected;

    CinemachineVirtualCamera virtualCamera;

    public CinemachineVirtualCamera VCam => virtualCamera;

    protected override void OnInitialize()
    {
        logger = FindAnyObjectByType<Logger>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

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
            
            if (UserName != DefaultName)
            {
                //UserName = $"{UserName}_{id}";
                deco.SetName($"{UserName}_{id}");
                UserName = UserName;
            }
            else
            {
                //UserName = $"{DefaultName}_{id}";
                deco.SetName($"{DefaultName}_{id}");
                UserName = DefaultName;
            }

            if(UserColor != Color.clear)
            {
                deco.SetColor(UserColor);
            }

            foreach (var other in NetworkManager.SpawnManager.SpawnedObjectsList)
            {                
                NetPlayer otherPlayer = other.GetComponent<NetPlayer>();
                if (otherPlayer != null && otherPlayer != player)
                {                    
                    otherPlayer.gameObject.name = $"OtherPlayer_{other.OwnerClientId}";
                }

                NetPlayerDecorator netDeco = other.GetComponent<NetPlayerDecorator>();
                if(netDeco != null && netDeco != deco)
                {
                    netDeco.RefreshNamePlate();
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
        //// Owner가 나가는 경우에는 실행안됨
        //NetworkObject netObj = NetworkManager.SpawnManager.GetPlayerNetworkObject(id);
        //if (netObj.IsOwner)
        //{
        //    // 
        //    deco.SetColor(Color.clear);
        //    deco = null;
        //    player = null;
        //}

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