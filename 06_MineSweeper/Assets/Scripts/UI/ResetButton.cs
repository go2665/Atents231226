using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    enum ButtonState
    {
        Normal = 0,
        Surprise,
        GameClear,
        GameOver
    };

    ButtonState state = ButtonState.Normal;
    ButtonState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                image.sprite = buttonSprites[(int)state];
            }

        }
    }

    public Sprite[] buttonSprites;

    Button button;
    Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);

        GameManager gameManager = GameManager.Instance;
        gameManager.Board.onBoardLeftPress += () =>
        {
            State = ButtonState.Surprise;
        };
        gameManager.Board.onBoardLeftRelease += () =>
        {
            State = ButtonState.Normal;
        };

        gameManager.onGameOver += () => State = ButtonState.GameOver;
        gameManager.onGameClear += () => State = ButtonState.GameClear;

    }

    void OnClick()
    {
        State = ButtonState.Normal;
        GameManager.Instance.GameReset();
    }

}

// 게임 오버가 되면 리셋버튼의 이미지가 Reset_GameOver로 변경된다.
// 게임 클리어가 되면 리셋버튼의 이미지가 Reset_Clear로 변경된다.