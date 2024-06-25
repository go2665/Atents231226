using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids.HostSimple
{
    // 서버에서 끊겼을 때 메뉴씬으로 보내는 클래스
    public class OnServerDisconnected : MonoBehaviour, INetworkRunnerCallbacks
    {
        // 메뉴씬의 이름
        [SerializeField] private string _menuSceneName = String.Empty;

        // 러너가 셧다운되었을 때 실행됨
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            // 로컬 네트워크 러너가 셧다운되면 메뉴씬이 로딩된다.
            SceneManager.LoadScene(_menuSceneName);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        // 러너가 호스트에서 연결이 끊어졌을 때 실행됨
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            // 클라이언트가 서버에서 끊기면 러너를 셧다운 시킨다.
            GetComponent<NetworkRunner>().Shutdown();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
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
    }
}
