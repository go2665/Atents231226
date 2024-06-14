using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(MazeVisualizer))]
[RequireComponent (typeof(NavMeshSurface))]
public class MazeGenerator : MonoBehaviour
{
    public int seed = -1;
    public enum MazeAlgorithm
    {
        RecursiveBackTracking = 0,
        Eller,
        Wilson
    }

    public MazeAlgorithm mazeAlgorithm = MazeAlgorithm.Wilson;

    MazeVisualizer visualizer;
    NavMeshSurface navMeshSurface;
    AsyncOperation navAsync;

    /// <summary>
    /// 생성한 미로
    /// </summary>
    Maze maze = null;

    public Maze Maze => maze;

    /// <summary>
    /// 미로의 골인 지점
    /// </summary>
    Goal goal;

    /// <summary>
    /// 미로 생성이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onMazeGenerated;

    private void Awake()
    {
        visualizer = GetComponent<MazeVisualizer>();
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void Generate(int width, int height)
    {
        switch (mazeAlgorithm)
        {
            case MazeAlgorithm.RecursiveBackTracking:
                maze = new BackTracking();
                break;
            case MazeAlgorithm.Eller:
                maze = new Eller();
                break;
            case MazeAlgorithm.Wilson:
                maze = new Wilson();
                break;
        }

        maze.MakeMaze(width, height, seed);

        visualizer.Clear();
        visualizer.Draw(maze);



        StartCoroutine(UpdateSurface());

    }

    IEnumerator UpdateSurface()
    {
        navAsync = navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        while (!navAsync.isDone)
        {
            yield return null;
        }
        Debug.Log("Nav Surface Updated!");

        // 오클루전 컬링도 새로 베이크 필요        

        onMazeGenerated?.Invoke();  // 미로 생성이 끝났음을 알림
    }
}
