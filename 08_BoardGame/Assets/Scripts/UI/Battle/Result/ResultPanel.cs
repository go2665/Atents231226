using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ResultPanel : MonoBehaviour
{
    // OpenButton을 누를 때마다 ResultBoard가 열리거나 닫힌다.
    // RestartButton을 누를 때 특정 기능이 실행된다.(함수만 연결해 놓기)
    // User나 Enemy가 패배하면 이 창이 열린다.
    // 열릴 때 누가 이겼느냐에 따라 ResultBoard의 세팅이 변경된다.
    // 열릴 때 ResultAnalysis의 데이터가 변경된다.

    ResultBoard board;
    ResultAnalysis userAnalysis;
    ResultAnalysis enemyAnalysis;

    UserPlayer user;
    EnemyPlayer enemy;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        Button openCloseBoard = child.GetComponent<Button>();

        child = transform.GetChild(1);
        board = child.GetComponent<ResultBoard>();

        child = board.transform.GetChild(1);
        userAnalysis = child.GetComponent<ResultAnalysis>();

        child = board.transform.GetChild(2);
        enemyAnalysis = child.GetComponent<ResultAnalysis>();

        child = board.transform.GetChild(3);
        Button restart = child.GetComponent<Button>();

        openCloseBoard.onClick.AddListener(board.Toggle);
        restart.onClick.AddListener(Restart);
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        user = gameManager.UserPlayer;
        enemy = gameManager.EnemyPlayer;

        user.onActionEnd += () =>
        {
            userAnalysis.TotalAttackCount = user.TotalAttackCount;
            userAnalysis.SuccessAttackCount = user.SuccessAttackCount;
            userAnalysis.FailAttackCount = user.FailAttackCount;
            userAnalysis.SuccessAttackRate = (float)user.SuccessAttackCount / (float)user.TotalAttackCount;
        };
        enemy.onActionEnd += () =>
        {
            enemyAnalysis.TotalAttackCount = enemy.TotalAttackCount;
            enemyAnalysis.SuccessAttackCount = enemy.SuccessAttackCount;
            enemyAnalysis.FailAttackCount = enemy.FailAttackCount;
            enemyAnalysis.SuccessAttackRate = (float)enemy.SuccessAttackCount / (float)enemy.TotalAttackCount;
        };

        user.onDefeat += () =>
        {
            board.SetVictoryDefeat(false);
            Open();
        };
        enemy.onDefeat += () =>
        { 
            board.SetVictoryDefeat(true); 
            Open(); 
        };

        Close();
    }

    /// <summary>
    /// ResultPanel을 여는 함수
    /// </summary>
    void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// ResultPanel을 닫는 함수
    /// </summary>
    void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 재시작 버튼을 누르면 실행될 함수
    /// </summary>
    void Restart()
    {

    }
}
