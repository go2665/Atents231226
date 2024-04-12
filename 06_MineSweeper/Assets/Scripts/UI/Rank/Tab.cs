using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    /// <summary>
    /// 선택되지 않았을 때의 색상
    /// </summary>
    readonly Color UnSelectedColor = new Color(0.44f, 0.44f, 0.44f, 1.0f);

    /// <summary>
    /// 선택되었을 때의 색상
    /// </summary>
    readonly Color SelectedColor = Color.white;

    /// <summary>
    /// 탭의 선택 여부(true면 선택됨, false면 선택되지 않음)
    /// </summary>
    bool isSelected = false;

    /// <summary>
    /// 탭의 선택 여부를 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            if(isSelected)
            {
                tabImage.color = SelectedColor;     // 선택된 경우
                SubPanelOpen();
                OnTabSelected?.Invoke(this);        // 선택되었음을 알림
            }
            else
            {
                tabImage.color = UnSelectedColor;   // 선택되지 않은 경우
                SubPanelClose();
            }
        }
    }

    /// <summary>
    /// 선택되었음을 알리는 델리게이트
    /// </summary>
    public Action<Tab> OnTabSelected;

    // 컴포넌트들
    Image tabImage;
    TabSubPanel subPanel;

    private void Awake()
    {
        tabImage = GetComponent<Image>();
        subPanel = GetComponentInChildren<TabSubPanel>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener( () => IsSelected = true );  // 버튼이 눌려지면 IsSelected는 true
    }

    /// <summary>
    /// 서브 패널을 여는 함수
    /// </summary>
    public void SubPanelOpen()
    {
        subPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 서브 패널을 닫는 함수
    /// </summary>
    public void SubPanelClose()
    {
        subPanel.gameObject.SetActive(false);
    }
}
