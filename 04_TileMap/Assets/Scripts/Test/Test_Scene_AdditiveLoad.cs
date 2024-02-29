using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Scene_AdditiveLoad : TestBase
{
    [Range(0, 2)]
    public int targetX = 0;

    [Range(0, 2)]
    public int targetY = 0;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene($"Seemless_{targetX}_{targetY}", LoadSceneMode.Additive);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        SceneManager.UnloadSceneAsync($"Seemless_{targetX}_{targetY}");
    }

}
