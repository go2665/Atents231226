using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishDeploymentButton : MonoBehaviour
{
    // 모든 함선이 배치되었을 때만 버튼이 활성화 됨

    Button button;
    UserPlayer player;

    GameManager gameManager;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        gameManager = GameManager.Instance;
        player = gameManager.UserPlayer;
        foreach(var ship in player.Ships)
        {
            ship.onDeploy += OnShipDeployed;    // 함선의 배치 정보가 변경될 때 OnShipDeployed를 실행
        }
    }

    /// <summary>
    /// 함선의 배치 정보가 변경될 때마다 실행되는 함수
    /// </summary>
    /// <param name="isDeployed">true면 지금 함선이 배치되었다. false면 지금 함선이 배치 취소 되었다.</param>
    private void OnShipDeployed(bool isDeployed)
    {
        if(isDeployed && player.IsAllDeployed)
        {
            button.interactable = true;     // 지금 배치되었고 모든 함선이 배치된 상황이면 버튼 활성화
        }
        else
        {
            button.interactable = false;    // 지금 함선이 배치 취소되었거나 모든 함선이 배치되지 않았으면 버튼 비활성화
        }
    }

    private void OnClick()
    {
        Debug.Log("Finish버튼 클릭 - 전투씬으로 넘어가야함");

        if( gameManager.SaveShipDeplyData() )
        {
            SceneManager.LoadScene(2);
        }
    }
}
