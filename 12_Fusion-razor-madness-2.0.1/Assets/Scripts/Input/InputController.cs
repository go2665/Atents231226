using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class InputController : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Networked]
    private NetworkButtons _prevData { get; set; }
    public NetworkButtons PrevButtons { get => _prevData; set => _prevData = value; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Runner.AddCallbacks(this);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        InputData currentInput = new InputData();
        
        currentInput.Buttons.Set(InputButton.RESPAWN, Input.GetKey(KeyCode.R));
        currentInput.Buttons.Set(InputButton.JUMP, Input.GetKey(KeyCode.Space));
        currentInput.Buttons.Set(InputButton.LEFT, Input.GetKey(KeyCode.A));
        currentInput.Buttons.Set(InputButton.RIGHT, Input.GetKey(KeyCode.D));

        input.Set(currentInput);
    }

    #region UnusedCallbacks
    public void OnConnectedToServer(NetworkRunner runner) {}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {}
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){ }

    public void OnSceneLoadDone(NetworkRunner runner) {}
    public void OnSceneLoadStart(NetworkRunner runner) {}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    #endregion
}
