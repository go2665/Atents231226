using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Maze : TestBase
{
    [Header("셀")]
    public Direction pathData;
    public CellVisualizer cellVisualizer;

    [Header("미로")]
    public int width = 5;
    public int height = 5;

    public MazeVisualizer backtracking;
    public MazeVisualizer eller;
    public MazeVisualizer wilson;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        cellVisualizer.RefreshWall((byte)pathData);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log(cellVisualizer.GetPath());
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        backtracking.Clear();

        BackTracking maze = new BackTracking();
        maze.MakeMaze(width, height, seed);
        backtracking.Draw(maze);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        eller.Clear();

        Eller maze = new Eller();
        maze.MakeMaze(width, height, seed);
        eller.Draw(maze);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        wilson.Clear();

        Wilson maze = new Wilson();
        maze.MakeMaze(width, height, seed);
        wilson.Draw(maze);
    }
}
