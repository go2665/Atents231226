using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    // 로딩 스크린
    // 1. 씬 로딩 진행도에 따라 슬라이더의 value가 변경된다(100%일 때 1)
    // 2. 로딩 중에는 LoadingText의 글자가 "Loading .", "Loading . .", "Loading . . .", "Loading . . . .", "Loading . . . . ."가 계속 반복된다.
    // 3. 로딩이 완료되면 LoadingText의 글자가 "Loading Complete!"로 변경되고 PressText가 활성화 된다.
    // 4. 로딩 진행도는 onMazeGenerated가 실행되었을 때 70%, onSpawnComplete가 실행되었을 때 100%
    // 5. 로딩이 완료되었을 때 아무 키보드 입력이나 마우스 클릭이 입력되면 로딩 스크린이 비활성화 된다.
    //      (추가로 GameManager.Instance.GameStart(); 실행)
    // 6. 씬 로딩 진행도는 목표치까지 꾸준히 증가한다.

    float currentProgress = 0.0f;
    float CurrentProgress
    {
        get => currentProgress;
        set
        {
            currentProgress = Mathf.Min(targetProgress, value);
            slider.value = currentProgress;

            if(currentProgress > 0.9999f)
            {
                OnLoadingComplete();
            }
        }
    }

    float targetProgress = 0.0f;

    string[] loadingStrings =
    {
        "Loading .", "Loading . .", "Loading . . .",
    };

    PlayerInputActions inputActions;

    Slider slider;
    TextMeshProUGUI loadingText;
    TextMeshProUGUI completeText;
    TextMeshProUGUI pressText;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        Transform child = transform.GetChild(0);
        loadingText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        completeText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        pressText = child.GetComponent<TextMeshProUGUI>();
        
        child = transform.GetChild(3);
        slider = child.GetComponent<Slider>();
        slider.value = 0.0f;
    }

    private void OnEnable()
    {
        inputActions.UI.AnyKey.performed += OnAnyKey;
    }

    private void OnDisable()
    {
        inputActions.UI.AnyKey.performed -= OnAnyKey;
        inputActions.UI.Disable();
    }

    private void Update()
    {
        CurrentProgress += Time.deltaTime;
    }

    public void Initialize()
    {
        CurrentProgress = 0.0f;
        targetProgress = 0.5f;
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine()
    {
        int index = 0;
        while(true)
        {
            loadingText.text = loadingStrings[index];
            index = (index + 1) % loadingStrings.Length;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnLoadginProgress(float progress)
    {
        targetProgress = progress;
        //Debug.Log($"Progress : {progress}");
    }

    void OnLoadingComplete()
    {
        loadingText.gameObject.SetActive(false);
        completeText.gameObject.SetActive(true);
        pressText.gameObject.SetActive(true);

        slider.value = 1;

        StopAllCoroutines();

        inputActions.UI.Enable();   // 입력 받기 시작
    }

    private void OnAnyKey(InputAction.CallbackContext context)
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.GameStart();
        Debug.Log("AnyKey");
    }
}
