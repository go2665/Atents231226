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
    private GameMode _gameMode;

    public string Nickname = "Player";
    public GameLauncher Launcher;

    public FusionEvent OnPlayerJoinedEvent;
    public FusionEvent OnPlayerLeftEvent;
    public FusionEvent OnShutdownEvent;
    public FusionEvent OnPlayerDataSpawnedEvent;

    [Space]
    [SerializeField] private GameObject _initPainel;
    [SerializeField] private GameObject _lobbyPainel;
    [SerializeField] private TextMeshProUGUI _lobbyPlayerText;
    [SerializeField] private TextMeshProUGUI _lobbyRoomName;
    [SerializeField] private Button _startButton;
    [Space]
    [SerializeField] private GameObject _modeButtons;
    [SerializeField] private TMP_InputField _nickname;
    [SerializeField] private TMP_InputField _room;

    private void OnEnable()
    {
        OnPlayerJoinedEvent.RegisterResponse(ShowLobbyCanvas);
        OnShutdownEvent.RegisterResponse(ResetCanvas);
        OnPlayerLeftEvent.RegisterResponse(UpdateLobbyList);
        OnPlayerDataSpawnedEvent.RegisterResponse(UpdateLobbyList);
    }

    private void OnDisable()
    {
        OnPlayerJoinedEvent.RemoveResponse(ShowLobbyCanvas);
        OnShutdownEvent.RemoveResponse(ResetCanvas);
        OnPlayerLeftEvent.RemoveResponse(UpdateLobbyList);
        OnPlayerDataSpawnedEvent.RemoveResponse(UpdateLobbyList);
    }

    //Called from button
    public void SetGameMode(int gameMode)
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Lobby);
        _gameMode = (GameMode)gameMode;
        _modeButtons.SetActive(false);
        _nickname.transform.parent.gameObject.SetActive(true);
    }

    //Called from button
    public void StartLauncher()
    {
        Launcher = FindObjectOfType<GameLauncher>();
        Nickname = _nickname.text;
        PlayerPrefs.SetString("Nick", Nickname);
        Launcher.Launch(_gameMode, _room.text);
        _nickname.transform.parent.gameObject.SetActive(false);
    }

    //Called from button
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    //Called from button
    public void LeaveLobby()
    {
        _ = LeaveLobbyAsync();
    }

    //Called from button
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
