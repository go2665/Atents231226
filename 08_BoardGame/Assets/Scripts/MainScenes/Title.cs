using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    //private void Start()
    //{
    //    // 게임 매니저가 없을 때 접근해서 새 게임 매니저를 만들고 그래서 함선 배치 씬에 있는 게임 매니저가 사라진다.
    //    GameManager.Instance.GameState = GameState.Title;       // 통일성 유지를 위해 넣은 것(실질적으로 하는일 없음)
    //}

    private void OnEnable()
    {
        inputActions.Title.Enable();
        inputActions.Title.Anything.performed += OnAnything;
    }

    private void OnDisable()
    {
        inputActions.Title.Anything.performed -= OnAnything;
        inputActions.Title.Disable();
    }

    private void OnAnything(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        SceneManager.LoadScene(1);  // 함선 배치 씬 로딩
    }
}
