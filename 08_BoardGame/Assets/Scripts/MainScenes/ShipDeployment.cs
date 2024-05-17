using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.GameState = GameState.ShipDeployment;
        gameManager.UserPlayer.BindInputFuncs();

        CinemachineVirtualCamera vcam = gameManager.GetComponentInChildren<CinemachineVirtualCamera>();
        vcam.m_Lens.OrthographicSize = 7.0f;

        gameManager.TurnController.TurnManagerStop();
    }
}
