using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Scene_AsyncLoad : TestBase
{
    public string nextSceneName = "LoadSampleScene";
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false; // 비동기 씬 로딩이 완료되어도 자동으로 씬 전환을 하지 않는다.
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true;  // 비동기 씬 로딩이 완료되면 자동으로 씬 전환을 한다.
    }

    IEnumerator LoadSceneCoroutine()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName); // 비동기 로딩 시작
        async.allowSceneActivation = false; // 자동으로 씬 전환되는 것 막기

        while(async.progress < 0.9f)    // allowSceneActivation가 false면 progress는 0.9가 최대(로딩완료 = 0.9 )
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }

        Debug.Log("Loading Complete");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        StartCoroutine(LoadSceneCoroutine());
    }
}
