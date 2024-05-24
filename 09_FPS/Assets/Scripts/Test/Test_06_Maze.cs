using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_Maze : TestBase
{
    public Direction pathData;
    public CellVisualizer cellVisualizer;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        cellVisualizer.RefreshWall((byte)pathData);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log(cellVisualizer.GetPath());
    }
}
