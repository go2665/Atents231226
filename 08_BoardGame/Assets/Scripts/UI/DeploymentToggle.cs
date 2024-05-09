using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeploymentToggle : MonoBehaviour
{
    Image image;
    GameObject deployEnd;

    readonly Color selectColor = new(1, 1, 1, 0.2f);

    enum DeployState : byte
    {
        NotSelect = 0,  // 선택되지 않은 상태
        Select,         // 선택된 상태(한번 클릭됨)
        Deployed        // 배치 완료된 상태
    }

    DeployState state = DeployState.NotSelect;

    DeployState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case DeployState.NotSelect:
                        image.color = Color.white;
                        deployEnd.SetActive(false);
                        break;
                    case DeployState.Select:
                        image.color = selectColor;
                        deployEnd.SetActive(false);
                        onSelect?.Invoke(this);
                        break;
                    case DeployState.Deployed:
                        image.color = selectColor;
                        deployEnd.SetActive(true);
                        break;
                }
            }
        }
    }

    public Action<DeploymentToggle> onSelect;

    private void Awake()
    {
        image = GetComponent<Image>();
        deployEnd = transform.GetChild(0).gameObject;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
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
                // 배치된 배를 배치 취소
                State = DeployState.NotSelect;
                break;
        }
    }

    public void SetNotSelect()
    {
        if(State != DeployState.Deployed)
        {
            State = DeployState.NotSelect;
        }
    }


#if UNITY_EDITOR
    public void Test_StateChange(int index)
    {
        State = (DeployState)index;
    }
#endif
}
