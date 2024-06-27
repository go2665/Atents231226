using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPrefabRef PlayerPrefab;
    public static Vector2 PlayerSpawnPos;
    
    private void Awake()
    {
        PlayerSpawnPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    /// <summary>
    /// Respawn all registred players on the level.
    /// </summary>
    /// <param name="runner"></param>
    public void RespawnPlayers(NetworkRunner runner)
    {
        if (!runner.IsClient)
        {
            PlayerSpawnPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
            foreach (var player in runner.ActivePlayers)
            {
                SpawnPlayer(runner, player, GameManager.Instance.GetPlayerData(player, runner).Nick.ToString());
            }
        }
    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player, string nick = "")
    {
        if (runner.IsServer)
        {
            NetworkObject playerObj = runner.Spawn(PlayerPrefab, PlayerSpawnPos, Quaternion.identity, player, InitializeObjBeforeSpawn);

            PlayerData data = GameManager.Instance.GetPlayerData(player, runner);
            data.Instance = playerObj;

            playerObj.GetComponent<PlayerBehaviour>().Nickname = data.Nick;
        }
    }

    private void InitializeObjBeforeSpawn(NetworkRunner runner, NetworkObject obj)
    {
        var behaviour = obj.GetComponent<PlayerBehaviour>();
        behaviour.PlayerColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
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
