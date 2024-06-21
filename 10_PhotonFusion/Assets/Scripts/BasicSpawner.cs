using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner myRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    /// <summary>
    /// 게임 세션을 열거나 접속하는 함수
    /// </summary>
    /// <param name="mode">게임에 접속하는 방식(Host or Client)</param>
    async void StartGame(GameMode mode) // async : 비동기 메서드임을 알림(내부에 await가 있음)
    {
        myRunner = this.gameObject.AddComponent<NetworkRunner>(); // 네트워크 러너 컴포넌트 추가
        myRunner.ProvideInput = true;                             // 유저 입력을 제공할 것이라고 설정

        // 현재 씬을 기반으로 NetworkSceneInfo 생성
        SceneRef scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        if(scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await myRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    /// <summary>
    /// GUI를 그리기 위한 이벤트 함수
    /// </summary>
    private void OnGUI()
    {
        if(myRunner == null)
        {
            if(GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Client"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    /// <summary>
    /// 플레이어가 접속했을 때 실행되는 함수
    /// </summary>
    /// <param name="runner">자기 자신의 러너(로 추정)</param>
    /// <param name="player">접속한 플레이어</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    { 
        if(runner.IsServer) // 서버에서만 실행
        {
            // 스폰 될 위치를 구하기
            Vector3 spawnPosition = new Vector3(player.RawEncoded % runner.Config.Simulation.PlayerCount , 0, 0);

            // 플레이어 오브젝트 생성(네번째 파라메터 : 이 오브젝트에 입력을 줄 수 있는 플레이어에 대한 참조(오너 같은 느낌)
            NetworkObject netPlayer = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            
            // 플레이어 전체 확인 및 접근을 편하게 하기 위한 용도
            spawnedCharacters.Add(player, netPlayer);
        }
    }

    /// <summary>
    /// 플레이어가 접속을 해제했을 때 실행되는 함수
    /// </summary>
    /// <param name="runner">자기 자신의 러너(로 추정)</param>
    /// <param name="player">접속 해제한 플레이어</param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    { 
        if(spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))  // spawnedCharacters에 player가 있을 때만 처리
        {
            runner.Despawn(networkObject);      // 러너에서 디스폰(게임 오브젝트 삭제도 함께 처리)
            spawnedCharacters.Remove(player);   // 딕셔너리에서 제거
        }
    }

    /// <summary>
    /// 사용자의 입력데이터를 수집하는 함수
    /// </summary>
    /// <param name="runner">네트워크 러너</param>
    /// <param name="input">데이터를 받아갈 변수</param>
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    { 
        NetworkInputData data = new NetworkInputData(); // 우리가 만든 데이터타입을 new하기

        if(Input.GetKey(KeyCode.W)) // 이 순간 W키가 눌려져 있는지 확인(결과가 true면 W키가 눌려져 있다. false면 안눌려져 있다)
        {
            data.direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            data.direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.direction += Vector3.right;
        }

        input.Set(data);    // 결정된 입력을 서버쪽으로 전달
    }

    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}
