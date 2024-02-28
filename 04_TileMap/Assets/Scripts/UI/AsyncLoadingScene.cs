using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    /// <summary>
    /// 다음에 로딩씬이 끝나고 나서 불려질 씬의 이름
    /// </summary>
    public string nextSceneName = "LoadSampleScene";

    /// <summary>
    /// 유니티에서 비동기 명령 처리를 위해 필요한 클래스
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// slider의 value에 영향을 줄 값
    /// </summary>
    float loadRatio;

    /// <summary>
    /// slider의 value가 증가하는 속도(초당)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// 글자 변경용 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// 로딩 완료 표시(true면 완료, false 미완)
    /// </summary>
    bool loadingDone = false;

    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;

    // 입력 처리용
    PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());
    }

    private void Update()
    {
        // 슬라이더의 value는 loadRatio가 될 때까지 계속 증가
        if(loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    /// <summary>
    /// 마우스나 키가 눌러지면 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void Press(InputAction.CallbackContext _)
    {
        //if (loadingDone)
        //    async.allowSceneActivation = true;

        async.allowSceneActivation = loadingDone;   // loadingDone이 true면 allowSceneActivation을 true로 만들기
    }

    /// <summary>
    /// 글자의 모양을 계속 변경하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {        
        // 0.2초 간격으로 .이 찍힌다.
        // .은 최대 5개까지만 찍인다.
        // "Loading" ~ "Loading . . . . ."

        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . .",
        };

        int index = 0;
        while(true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// 비동기로 씬을 로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        async = SceneManager.LoadSceneAsync(nextSceneName); // 비동기 로딩 시작
        async.allowSceneActivation = false;                 // 자동으로 씬전환되지 않도록 하기

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;  // 로딩 진행율에 따라 loadRatio 설정
            yield return null;
        }

        // 남아있는 슬라이더가 다 찰 때까지 기다리기
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);        // 글자 변경 안되게 만들기
        loadingText.text = "Loading\nComplete!";    // 완료되었다고 글자 출력
        loadingDone = true;                         // 로딩 완료되었다고 표시
    }
}