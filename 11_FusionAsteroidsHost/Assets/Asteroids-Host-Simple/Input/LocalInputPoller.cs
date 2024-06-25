using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Asteroids.HostSimple
{
    public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
    {
        private const string AXIS_HORIZONTAL = "Horizontal";
        private const string AXIS_VERTICAL = "Vertical";
        private const string BUTTON_FIRE1 = "Fire1";
        private const string BUTTON_JUMP = "Jump"; // 스페이스 키로 발사를 하고 싶다면 이것을 사용해도 됨

        // LocalInputPoller는 NetworkRunner와 같은 게임 오브젝트에 들어있기 때문에 AddCallbak을 하지 않아도 자동으로 감지된다.

        // 서버에게 입력 전달
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            SpaceshipInput localInput = new SpaceshipInput();

            localInput.HorizontalInput = Input.GetAxis(AXIS_HORIZONTAL);
            localInput.VerticalInput = Input.GetAxis(AXIS_VERTICAL);
            localInput.Buttons.Set(SpaceshipButtons.Fire, Input.GetButton(BUTTON_FIRE1));
            //localInput.Buttons.Set(SpaceshipButtons.Fire, Input.GetButton(BUTTON_JUMP));

            input.Set(localInput);
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

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
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
