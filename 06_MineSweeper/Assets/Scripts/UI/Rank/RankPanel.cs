using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 이 패널이 가진 모든 탭
    /// </summary>
    Tab[] tabs;
    
    /// <summary>
    /// 이 패널이 현재 선택한 탭
    /// </summary>
    Tab selectedTab;

    /// <summary>
    /// 선택한 탭을 설정하고 확인하는 프로퍼티
    /// </summary>
    Tab SelectedTab
    {
        get => selectedTab;
        set
        {
            if(value != selectedTab)
            {
                selectedTab.IsSelected = false; // 이전 것은 선택해제
                selectedTab = value;
                selectedTab.IsSelected = true;  // 새것을 선택
            }

        }
    }

    /// <summary>
    /// 이 패널이 사용하는 토글 버튼(탭의 서브패널 열고 닫기용)
    /// </summary>
    ToggleButton toggle;

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();
        foreach(Tab tab in tabs)
        {
            tab.OnTabSelected += (newSelected) =>
            {
                SelectedTab = newSelected;
                toggle.ToggleOn();
            };
        }

        toggle = GetComponentInChildren<ToggleButton>();
        toggle.onToggleChange += (isOn) =>
        {
            if(isOn)
            {
                SelectedTab.SubPanelOpen();
            }
            else
            {
                SelectedTab.SubPanelClose();
            }
        };

        selectedTab = tabs[tabs.Length - 1];    // 선택된 탭은 무조건 존재(처음 열릴 때 문제가 발생하는 것 방지용)
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.onGameClear += Open;    // 게임 Clear일 때 열기
        gameManager.onGameReady += Close;   // 게임 Close일 때 닫기

        Close();    // 시작하자 마다 닫기
    }

    /// <summary>
    /// 랭크 패널을 여는 함수
    /// </summary>
    void Open()
    {
        SelectedTab = tabs[0];
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 랭크 패널을 닫는 함수
    /// </summary>
    void Close()
    {
        gameObject.SetActive(false);
    }
}
