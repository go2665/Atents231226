using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    Tab[] tabs;
    Tab selectedTab;

    Tab SelectedTab
    {
        get => selectedTab;
        set
        {

        }
    }

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();
    }

    /// <summary>
    /// 랭크 패널을 여는 함수
    /// </summary>
    void Open()
    {
        // 게임이 Clear되면 실행된다.
    }

    /// <summary>
    /// 랭크 패널을 닫는 함수
    /// </summary>
    void Close()
    {
        // 게임이 Ready가 되면 항상 실행된다.
    }
}
