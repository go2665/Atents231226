using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameManager.Instance.GameReset();
    }

}

// 셀을 누르면 리셋버튼의 이미지가 Reset_Surprise로 변경된다.
// 게임 오버가 되면 리셋버튼의 이미지가 Reset_GameOver로 변경된다.
// 게임 클리어가 되면 리셋버튼의 이미지가 Reset_Clear로 변경된다.