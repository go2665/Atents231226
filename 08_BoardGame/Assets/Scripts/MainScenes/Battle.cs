using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.GameState = GameState.Battle;

        gameManager.UserPlayer.BindInputFuncs();

        CinemachineVirtualCamera vcam = gameManager.GetComponentInChildren<CinemachineVirtualCamera>();
        vcam.m_Lens.OrthographicSize = 10.0f;


        // 함선 배치(저장해 놓은 것 로딩 시도 후 실패하면 자동 배치)
        if (!gameManager.LoadShipDeployData())
        {
            gameManager.UserPlayer.AutoShipDeployment(true);
        }
        gameManager.EnemyPlayer.AutoShipDeployment(gameManager.IsTestMode);
    }
}
