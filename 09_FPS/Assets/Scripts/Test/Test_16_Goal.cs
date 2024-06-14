using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_16_Goal : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Enemy enemy = FindAnyObjectByType<Enemy>();
        enemy.HP -= 10000;             
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();

        for (int i = 0; i < 10000000; i++)
        {
            Vector2Int result = goal.TestSetRandomPosition(GameManager.Instance.MazeWidth, GameManager.Instance.MazeHeight);
            if(result.x == 1 && result.y == 1)
            {
                Debug.LogError("Not Valid");
            }

        }
        Debug.Log("Check complete");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Goal goal = FindAnyObjectByType<Goal>();
        goal.SetRandomPosition(GameManager.Instance.MazeWidth, GameManager.Instance.MazeHeight);
    }
}
