using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_20_Main : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.GameClear();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }
}
