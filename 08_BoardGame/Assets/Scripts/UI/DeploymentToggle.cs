using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    /// <summary>
    /// 이 버튼에서 처리할 함선의 종류
    /// </summary>
    public ShipType shipType = ShipType.None;

    /// <summary>
    /// 버튼 이미지
    /// </summary>
    Image image;

    /// <summary>
    /// 배치 완료 표시용 게임 오브젝트
    /// </summary>
    GameObject deployEnd;

    /// <summary>
    /// 버튼이 선택되었을 때 보일 색상
    /// </summary>
    readonly Color selectColor = new(1, 1, 1, 0.2f);

    /// <summary>
    /// 버튼의 상태 종류
    /// </summary>
    enum DeployState : byte
    {
        NotSelect = 0,  // 선택되지 않은 상태
        Select,         // 선택된 상태(한번 클릭됨)
        Deployed        // 배치 완료된 상태
    }

    /// <summary>
    /// 버튼의 현재 상태
    /// </summary>
    DeployState state = DeployState.NotSelect;

    /// <summary>
    /// 버튼의 상태 확인 및 설정용 프로퍼티
    /// </summary>
    DeployState State
    {
        get => state;
        set
        {
            if(state != value)  // 상태가 변경되었을 때만 처리
            {
                state = value;
                switch (state)
                {
                    case DeployState.NotSelect:             // 선택안됨 상태일 때
                        image.color = Color.white;          // 색상을 원상 복귀
                        deployEnd.SetActive(false);         // 배치 완료 표시 안보이게 하기
                        player.UndoShipDeploy(shipType);    // 함선이 배치된 상태면 배치 취소
                        break;
                    case DeployState.Select:                // 선택됨 상태일 때
                        image.color = selectColor;          // 색상을 반투명하게 만들기
                        deployEnd.SetActive(false);         // 배치 완료 표시 안보이게 하기
                        player.SelectShipToDeploy(shipType);    // 함선을 배치하기 위해 선택하고
                        onSelect?.Invoke(this);             // 선택되었음을 알림
                        break;
                    case DeployState.Deployed:              // 배치 완료 상태일 때
                        image.color = selectColor;          // 색상은 반투명하게 보이게 하기
                        deployEnd.SetActive(true);          // 배치 완료 표시 보이게 하기
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 선택되었을 때 실행될 델리게이트
    /// </summary>
    public Action<DeploymentToggle> onSelect;

    /// <summary>
    /// 함선들을 가진 유저 플레이어
    /// </summary>
    UserPlayer player;

    private void Awake()
    {
        image = GetComponent<Image>();
        deployEnd = transform.GetChild(0).gameObject;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        player = GameManager.Instance.UserPlayer;       // 플레이어 저장하기

        Ship targetShip = player.GetShip(shipType);     // 이 버튼과 묶인 함선 찾기
        if (targetShip != null)
        {
            targetShip.onDeploy += (isDeploy) =>        // 그 함선이 배치되거나 배치해제되면 실행
            {
                if (isDeploy)
                {
                    State = DeployState.Deployed;       // 함선이 배치된 상태면 버튼도 배치 상태로 변경
                }
                else
                {
                    State = DeployState.NotSelect;      // 함선이 배치 해제된 상태면 버튼은 NotSelect로 변경
                }
            };
        }
    }

    /// <summary>
    /// 버튼이 클릭되면 실행되는 함수
    /// </summary>
    private void OnClick()
    {
        // 상태별로 다른 상태로 전환되기
        switch(state)
        {
            case DeployState.NotSelect:
                State = DeployState.Select;
                // 함선 배치 모드로 전환
                break;
            case DeployState.Select:
                State = DeployState.NotSelect;
                break;
            case DeployState.Deployed:
                State = DeployState.NotSelect;
                // 배치된 배를 배치 취소
                break;
        }
    }

    /// <summary>
    /// 한 종류의 배치 버튼이 눌려졌을 때 다른 버튼들을 선택 해제 상태로 만들기 위해 사용되는 함수
    /// </summary>
    public void SetNotSelect()
    {
        if(State != DeployState.Deployed)       // 배치 상태면
        {
            State = DeployState.NotSelect;      // 선택 안됨 상태로 변경
        }
    }


#if UNITY_EDITOR
    public void Test_StateChange(int index)
    {
        State = (DeployState)index;
    }
#endif
}
