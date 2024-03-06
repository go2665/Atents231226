using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 1.0f;
    CanvasGroup canvasGroup;
    TextMeshProUGUI playTime;
    TextMeshProUGUI killCount;
    Button restart;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(1);
        playTime = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        killCount = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        restart = child.GetComponent<Button>();

        restart.onClick.AddListener( () =>
        {
            StartCoroutine(WaitUnloadAll());
        });      // restart버튼이 눌려지면 AddListener로 등록한 함수가 실행된다.
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Player player = GameManager.Instance.Player;
        player.onDie += OnPlayerDie;
    }

    private void OnPlayerDie(float totalPlayTime, int totalKillCount)
    {
        playTime.text = $"Total Play Time\n\r< {totalPlayTime:f1} Sec >";
        killCount.text = $"Total Kill Count\n\r< {totalKillCount} Kill >";
        StartCoroutine(StartAlphaChange());
    }

    IEnumerator StartAlphaChange()
    {
        while(canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator WaitUnloadAll()
    {
        WorldManager world = GameManager.Instance.World;
        while(!world.IsUnloadAll)   // 플레이어가 죽었을 때 모든 맵을 로딩해제 요청을 하니까 다 될 때까지 대기
        {
            yield return null;
        }
        SceneManager.LoadScene("AsyncLoadScene");
    }
}
