using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_09_PlayerBase : TestBase
{
    public Button reset;
    public Button random;
    public Button resetAndRandom;

    public PlayerBase user;
    public PlayerBase enemy;

    private void Start()
    {
        user = GameManager.Instance.UserPlayer;
        enemy = GameManager.Instance.EnemyPlayer;

        reset.onClick.AddListener( user.Clear );
        reset.onClick.AddListener( enemy.Clear );

        bool isShow = GameManager.Instance.IsTestMode;

        random.onClick.AddListener(() =>
        {
            user.AutoShipDeployment(true);
            enemy.AutoShipDeployment(isShow);
        });

        resetAndRandom.onClick.AddListener( () =>
        {
            user.Clear();
            user.AutoShipDeployment(true);
            enemy.Clear();
            enemy.AutoShipDeployment(isShow);
        });

        user.AutoShipDeployment(true);
        enemy.AutoShipDeployment(isShow);
    }

    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        // 보드 공격하기(유저, 적 상관없음)
        Vector2Int grid = user.Board.GetMouseGridPosition();
        enemy.Attack(grid);
        grid = enemy.Board.GetMouseGridPosition();
        user.Attack(grid);
    }

    protected override void OnTestRClick(InputAction.CallbackContext context)
    {
        // 그 위치에 배치된 배를 배치 해제(유저만)
        Vector2Int grid = user.Board.GetMouseGridPosition();
        ShipType type = user.Board.GetShipTypeOnBoard(grid);
        if(type != ShipType.None)
        {
            UserPlayer userPlayer = user as UserPlayer;
            userPlayer.UndoShipDeploy(type);
        }

#if UNITY_EDITOR
        Vector2Int enemyGrid = enemy.Board.GetMouseGridPosition();
        user.Test_IsSuccessLine(enemyGrid);
#endif
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        user.AutoAttack();
    }
}
