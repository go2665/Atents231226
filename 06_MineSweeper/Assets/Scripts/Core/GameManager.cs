using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 상태 관련 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 게임 상태 종류
    /// </summary>
    public enum GameState
    {
        Ready,          // 게임 시작 전
        Play,           // 첫번째 셀이 열리거나 깃발이 설치된 이후
        GameClear,      // 모든 지뢰를 찾았을 때
        GameOver        // 지뢰가 있는 셀을 열었을 때
    }

    /// <summary>
    /// 현재 게임 상태
    /// </summary>
    GameState state = GameState.Ready;

    /// <summary>
    /// 상태 변경 및 확인용 프로퍼티
    /// </summary>
    GameState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch(state)
                {
                    case GameState.Ready:
                        PlayerName = string.Empty;
                        FlagCount = mineCount;                        
                        onGameReady?.Invoke();      // 델리게이트 실행
                        break;
                    case GameState.Play:
                        ActionCount = 0;
                        if(PlayerName == string.Empty || PlayerName == "")
                        {
                            int test = 1234512345;
                            //DateTime.Now.GetHashCode()
                            PlayerName = $"Player{(uint)(test % 10000000)}";
                        }
                        Debug.Log($"시작할 때 플레이어 이름 : {PlayerName}");
                        onGamePlay?.Invoke();
                        break;
                    case GameState.GameClear:
                        onGameClear?.Invoke();
                        break;
                    case GameState.GameOver:
                        onGameOver?.Invoke();
                        break;
                }
            }
        }
    }

    public bool IsPlaying => State == GameState.Play;

    // 상태 변경 알림용 프로퍼티
    public Action onGameReady;
    public Action onGamePlay;
    public Action onGameClear;
    public Action onGameOver;

    // 보드 생성 관련 --------------------------------------------------------------------------------------
    
    /// <summary>
    /// 보드 생성시 지뢰 개수
    /// </summary>
    public int mineCount = 10;

    /// <summary>
    /// 보드 생성시 가로 길이
    /// </summary>
    public int boardWidth = 8;

    /// <summary>
    /// 보드 생성시 세로 길이
    /// </summary>
    public int boardHeight = 8;

    /// <summary>
    /// 게임 보드
    /// </summary>
    Board board;

    /// <summary>
    /// 게임 보드 확인용 프로퍼티
    /// </summary>
    public Board Board => board;


    // 깃발 관련 -------------------------------------------------------------------------------------------

    /// <summary>
    /// 깃발 개수
    /// </summary>
    int flagCount = 0;

    /// <summary>
    /// 깃발 개수 확인 및 설정용 프로퍼티
    /// </summary>
    public int FlagCount
    {
        get => flagCount;
        private set                 // 쓰기는 this에서만 가능
        {
            if(flagCount != value)  // 변경되었을 때만 실행
            {
                flagCount = value;
                onFlagCountChange?.Invoke(flagCount);   // 델리게이트로 변경된 값 알리기
            }
        }
    }

    /// <summary>
    /// 깃발 개수가 변경될 때마다 실행되는 델리게이트
    /// </summary>
    public Action<int> onFlagCountChange;

    /// <summary>
    /// 깃발 개수를 1 증가시키는 함수
    /// </summary>
    public void IncreaseFlagCount()
    {
        FlagCount++;
    }

    /// <summary>
    /// 깃발 개수를 1 감소시키는 함수
    /// </summary>
    public void DecreaseFlagCount()
    {
        FlagCount--;
    }

    // 플레이 행동 관련 ---------------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어 행동 회수
    /// </summary>
    int actionCount = 0;
    public int ActionCount
    {
        get => actionCount;
        private set
        {
            if (actionCount != value) 
            {
                actionCount = value;
                onActionCountChange?.Invoke(actionCount);   // 행동 회수가 변경되었다고 알림
            }
        }
    }

    /// <summary>
    /// 행동 회수 변화를 알리는 델리게이트
    /// </summary>
    public Action<int> onActionCountChange;

    /// <summary>
    /// 플래이어의 행동이 끝났을 때 실행될 함수
    /// </summary>
    public void PlayerActionEnd()
    {
        ActionCount++;
    }

    // 시간 관련 ---------------------------------------------------------------------------------------
    /// <summary>
    /// 타이머
    /// </summary>
    Timer timer;

    /// <summary>
    /// 현재 플레이 진행 시간
    /// </summary>
    public float PlayTime => timer.ElapsedTime;

    // 게임 상태 관련 -----------------------------------------------------------------------------------
    public void GameStart()
    {
        if( State == GameState.Ready )  // 레디 상태일 때만 시작 가능
        {
            State = GameState.Play;
        }
    }

    public void GameReset()
    {
        State = GameState.Ready;
    }

    public void GameOver()
    {
        State = GameState.GameOver;
    }

    public void GameClear()
    {
        State = GameState.GameClear;
        Debug.Log("Game Clear");

        // 깃발이 다 설치되어있고
        // 지뢰가 아닌 셀이 모두 열려 있어야 한다.
    }

    // 플레이어 정보용 ----------------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어 이름 입력용 UI
    /// </summary>
    PlayerNameInput playerNameInput;

    /// <summary>
    /// 플레이어의 이름을 설정하고 확인하기 위한 프로퍼티
    /// </summary>
    public string PlayerName
    {
        get => playerNameInput?.GetPlayerName();
        private set => playerNameInput?.SetPlayerName(value);
    }

    // 랭킹 관련 ---------------------------------------------------------------------------------------
    RankDataManager rankDataManager;
    public RankDataManager RankDataManager => rankDataManager;

    // 게임 메니저 공용 함수 ----------------------------------------------------------------------------
    protected override void OnInitialize()
    {
        rankDataManager = GetComponent<RankDataManager>();

        // 보드 초기화
        board = FindAnyObjectByType<Board>();
        board.Initialize(boardWidth, boardHeight, mineCount);   

        FlagCount = mineCount;  // 깃발 개수 설정
        timer = FindAnyObjectByType<Timer>();   // 타이머 찾기

        playerNameInput = FindAnyObjectByType<PlayerNameInput>();
    }


#if UNITY_EDITOR
    public void Test_SetFlagCount(int flagCount)
    {
        FlagCount = flagCount;
    }

    public void Test_StateChage(GameState state)
    {
        State = state;
    }
#endif
}
