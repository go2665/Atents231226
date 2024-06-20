using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    TextMeshProUGUI title;
    TextMeshProUGUI kill;
    TextMeshProUGUI time;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        title = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        kill = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        time = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        Button restart = child.GetComponent<Button>();
        restart.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    /// <summary>
    /// 게임이 끝날 때 열리는 창
    /// </summary>
    /// <param name="isClear">true면 출구로 나감. false 적에게 죽음</param>
    /// <param name="killCount">죽인 적 수</param>
    /// <param name="playTime">전체 플레이 타임</param>
    public void Open(bool isClear, int killCount, float playTime)
    {
        if(isClear)
        {
            title.text = "Game Clear";
        }
        else
        {
            title.text = "Game Over";
        }

        kill.text = killCount.ToString();
        time.text = playTime.ToString("f1");
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
    }
}
