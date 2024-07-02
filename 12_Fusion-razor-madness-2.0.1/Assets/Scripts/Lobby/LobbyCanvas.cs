using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using FusionUtilsEvents;
using System.Threading.Tasks;

public class LobbyCanvas : MonoBehaviour
{
    // 시작할 게임의 모드(Single, Host, Client 중 하나)
    private GameMode _gameMode;

    // 플레이어 이름(의미없음)
    public string Nickname = "Player";

    // 게임 세션을 시작시키는 런처
    public GameLauncher Launcher;

    public FusionEvent OnPlayerJoinedEvent;
    public FusionEvent OnPlayerLeftEvent;
    public FusionEvent OnShutdownEvent;
    public FusionEvent OnPlayerDataSpawnedEvent;

    [Space]
    // 처음 시작할 때 보이는 패널
    [SerializeField] private GameObject _initPainel;
    // 로비(방)에 들어갔을 때 보이는 패널
    [SerializeField] private GameObject _lobbyPainel;
    // 플레이어 목록 표시하는 텍스트
    [SerializeField] private TextMeshProUGUI _lobbyPlayerText;
    // 로비(방) 이름
    [SerializeField] private TextMeshProUGUI _lobbyRoomName;
    // 로비에 있는 게임 시작 버튼
    [SerializeField] private Button _startButton;
    [Space]
    // 접속 모드 선택 버튼들을 모아 놓은 게임 오브젝트
    [SerializeField] private GameObject _modeButtons;
    // 이름 입력용 인풋 필드
    [SerializeField] private TMP_InputField _nickname;
    // 로비(방) 이름 입력용 인풋필드
    [SerializeField] private TMP_InputField _room;

    private void OnEnable()
    {
        OnPlayerJoinedEvent.RegisterResponse(ShowLobbyCanvas);          // 플레이어가 들어오면 ShowLobbyCanvas 실행
        OnShutdownEvent.RegisterResponse(ResetCanvas);                  // 셧다운 될 때 ResetCanvas 실행
        OnPlayerLeftEvent.RegisterResponse(UpdateLobbyList);            // 플레이어가 나가면 로비 리스트 갱신
        OnPlayerDataSpawnedEvent.RegisterResponse(UpdateLobbyList);     // 플레이어가 스폰되면 로비 리스트 갱신
    }

    private void OnDisable()
    {
        OnPlayerJoinedEvent.RemoveResponse(ShowLobbyCanvas);
        OnShutdownEvent.RemoveResponse(ResetCanvas);
        OnPlayerLeftEvent.RemoveResponse(UpdateLobbyList);
        OnPlayerDataSpawnedEvent.RemoveResponse(UpdateLobbyList);
    }

    // 모드 버튼 눌렀을 때 실행되는 함수
    public void SetGameMode(int gameMode)
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Lobby); // 게임 상태를 로비 상태로 변경
        _gameMode = (GameMode)gameMode; // 눌려진 게임 모드 저장
        _modeButtons.SetActive(false);  // 모드 버튼들 전부 비활성화
        _nickname.transform.parent.gameObject.SetActive(true);  // 방 이름과 플레이어 이름을 설정할 수 있도록 활성화
    }

    // 플레이어 이름과 방이름을 설정하고 아래의 OK버튼을 눌렀을 때 실행되는 함수
    public void StartLauncher()
    {
        Launcher = FindObjectOfType<GameLauncher>();    // 런처 찾고
        Nickname = _nickname.text;
        PlayerPrefs.SetString("Nick", Nickname);        // 이름 저장하고
        Launcher.Launch(_gameMode, _room.text);         // 입력된 방이름과 게임모드로 세션 시작
        _nickname.transform.parent.gameObject.SetActive(false); // 방이름과 플레이어 이름 설정창 안보이게 만들기
    }

    // 종료버튼
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    // 로비에서 Back버튼 눌렀을 때 실행되는 함수
    public void LeaveLobby()
    {
        _ = LeaveLobbyAsync();
    }

    // 로비에서 Start버튼 눌렀을 때 실행되는 함수
    public void StartButton()
    {
        FusionHelper.LocalRunner.SessionInfo.IsOpen = false;
        FusionHelper.LocalRunner.SessionInfo.IsVisible = false;
        LoadingManager.Instance.LoadNextLevel(FusionHelper.LocalRunner);
    }

    private async Task LeaveLobbyAsync()
    {
        if (FusionHelper.LocalRunner.IsServer)
        {
            CloseLobby();
        }
        await FusionHelper.LocalRunner?.Shutdown();
    }

    public void CloseLobby()
    {
        foreach(var player in FusionHelper.LocalRunner.ActivePlayers)
        {
            if (player!= FusionHelper.LocalRunner.LocalPlayer)
                FusionHelper.LocalRunner.Disconnect(player);
        }
    }

    private void ResetCanvas(PlayerRef player, NetworkRunner runner)
    {
        _initPainel.SetActive(true);
        _modeButtons.SetActive(true);
        _lobbyPainel.SetActive(false);
        _startButton.gameObject.SetActive(runner.IsServer);
    }

    public void ShowLobbyCanvas(PlayerRef player, NetworkRunner runner)
    {
        _initPainel.SetActive(false);
        _lobbyPainel.SetActive(true);
    }

    public void UpdateLobbyList(PlayerRef playerRef, NetworkRunner runner)
    {
        _startButton.gameObject.SetActive(runner.IsServer);
        string players = default;
        string isLocal;
        foreach(var player in runner.ActivePlayers)
        {
            isLocal = player == runner.LocalPlayer ? " (You)" : string.Empty;
            players += GameManager.Instance.GetPlayerData(player, runner)?.Nick + isLocal + " \n";
        }
        _lobbyPlayerText.text = players;
        _lobbyRoomName.text = $"Room: {runner.SessionInfo.Name}";
    }
}
