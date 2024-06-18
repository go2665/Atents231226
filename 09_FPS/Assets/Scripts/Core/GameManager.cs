using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player => player;

    /// <summary>
    /// 플레이어를 따라다니는 카메라
    /// </summary>
    CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera FollowCamera => followCamera;

    /// <summary>
    /// 적 스포너
    /// </summary>
    EnemySpawner spawner;
    public EnemySpawner Spawner => spawner;

    /// <summary>
    /// 미로 크기
    /// </summary>
    public int mazeWidth = 20;
    public int mazeHeight = 20;

    /// <summary>
    /// 미로 크기
    /// </summary>
    public int MazeWidth => mazeWidth;
    public int MazeHeight => mazeHeight;

    /// <summary>
    /// 미로 생성기
    /// </summary>
    MazeGenerator mazeGenerator;

    /// <summary>
    /// 미로 확인용 프로퍼티
    /// </summary>
    public Maze Maze => mazeGenerator.Maze;

    /// <summary>
    /// 킬카운트
    /// </summary>
    int killCount = 0;

    /// <summary>
    /// 플레이 타임
    /// </summary>
    float playTime = 0.0f;

    /// <summary>
    /// 게임 시작을 알리는 델리게이트
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// 게임 종료를 알리는 델리게이트(bool:true면 클리어, false면 over)
    /// </summary>
    public Action<bool> onGameEnd;

    protected override void OnInitialize()
    {     
        player = FindAnyObjectByType<Player>();  
        player.onDie += GameOver;

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null)
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }

        spawner = FindAnyObjectByType<EnemySpawner>();        

        mazeGenerator = FindAnyObjectByType<MazeGenerator>();
        if(mazeGenerator != null)
        {
            mazeGenerator.Generate(MazeWidth, MazeHeight);
            mazeGenerator.onMazeGenerated += () =>
            {
                // 적 스폰
                spawner?.EnemyAll_Spawn();

                playTime = 0;   // 플레이 시간 초기화
                killCount = 0;  // 킬카운트 초기화
            };
        }

        ResultPanel resultPanel = FindAnyObjectByType<ResultPanel>();
        resultPanel.gameObject.SetActive(false);

        onGameEnd += (isClear) =>
        {
            //Time.timeSinceLevelLoad : 씬이 로딩되고 지난 시간
            Crosshair crosshair = FindAnyObjectByType<Crosshair>();
            crosshair.gameObject.SetActive(false);          // 크로스 해어 안보이게 만들기
            resultPanel.Open(isClear, killCount, playTime);
        };

        Cursor.lockState = CursorLockMode.Locked;   // 커서 안보이게 만들기
    }

    public void IncreaseKillCount()
    {
        killCount++;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }

    /// <summary>
    /// 게임 시작시 실행될 함수
    /// </summary>
    public void GameStart()
    {
        onGameStart?.Invoke();
    }

    /// <summary>
    /// 게임 클리어시 실행될 함수
    /// </summary>
    public void GameClear()
    {
        onGameEnd?.Invoke(true);
    }

    /// <summary>
    /// 게임 오버시 실행될 함수
    /// </summary>
    public void GameOver()
    {
        onGameEnd?.Invoke(false);
    }
}
