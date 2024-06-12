using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera FollowCamera => followCamera;

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

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();

        GameObject obj = GameObject.FindWithTag("FollowCamera");
        if (obj != null)
        {
            followCamera = obj.GetComponent<CinemachineVirtualCamera>();
        }

        mazeGenerator = FindAnyObjectByType<MazeGenerator>();
        if(mazeGenerator != null)
        {
            mazeGenerator.Generate(mazeWidth, mazeHeight);
            mazeGenerator.onMazeGenerated += () =>
            {
                // 플레이어를 미로의 가운데 위치로 옮기기
                Vector3 centerPos = MazeVisualizer.GridToWorld(mazeWidth / 2, mazeHeight / 2);
                player.transform.position = centerPos;
            };
        }
    }
}
