using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    // 플레이어의 프리팹
    public NetworkPrefabRef PlayerPrefab;

    // 스폰되는 위치(Respawn이라는 tag가 설정된 게임 오브젝트의 위치)
    public static Vector2 PlayerSpawnPos;
    
    private void Awake()
    {
        PlayerSpawnPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;    // 다른 리스폰 지점을 선정할 때를 대비?
        //PlayerSpawnPos = transform.position;
    }

    /// <summary>
    /// 등록된 플레이어 전부 리스폰
    /// </summary>
    /// <param name="runner">네트워크 러너</param>
    public void RespawnPlayers(NetworkRunner runner)
    {
        if (!runner.IsClient)   // 클라이언트가 아니면(호스트만 실행)
        {
            PlayerSpawnPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;    // 다시 스폰위치 찾기
            foreach (var player in runner.ActivePlayers)    // 러너에서 활성화되어있는 모든 플레이어에 대해
            {
                // 생성(러너, 플레이어 레퍼런스, 플레이어의 이름)
                SpawnPlayer(runner, player, GameManager.Instance.GetPlayerData(player, runner).Nick.ToString());
            }
        }
    }

    // 실제 스폰작업 처리(러너, 플레이어 레퍼런스, 이름)
    private void SpawnPlayer(NetworkRunner runner, PlayerRef player, string nick = "")
    {        
        if (runner.IsServer)    // 서버(=호스트)만 실행
        {
            NetworkObject playerObj = runner.Spawn(
                PlayerPrefab,               // 생성할 프리팹
                PlayerSpawnPos,             // 생성할 위치
                Quaternion.identity,        // 생성할 회전
                player,                     // 생성한 오브젝트의 입력권한을 가진 플레이어
                InitializeObjBeforeSpawn);  // 스폰전에 실행할 함수

            PlayerData data = GameManager.Instance.GetPlayerData(player, runner);
            data.Instance = playerObj;

            playerObj.GetComponent<PlayerBehaviour>().Nickname = data.Nick;
        }
    }

    // 스폰 전에 초기화 작업
    private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        // obj = 생성은 되었지만 아직 네트워크상에 스폰은 안된 오브젝트
        var behaviour = obj.GetComponent<PlayerBehaviour>();
        // 플레이어의 색상을 랜덤으로 정하기(32비트 크기의 컬러로 만들기)
        behaviour.PlayerColor = new Color32(
            (byte)Random.Range(0, 255), 
            (byte)Random.Range(0, 255), 
            (byte)Random.Range(0, 255), 
            255);
    }


    #region UnusedCallbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    #endregion
}
