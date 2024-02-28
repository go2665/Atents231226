using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SceneLoad : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("LoadSampleScene");        // 동기 방식(Synchronous) : 이 코드가 끝나야 다음 코드가 실행된다.
        // SceneManager.LoadScene(0);
    }
}
