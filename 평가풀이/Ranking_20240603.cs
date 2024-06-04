using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class Ranking_20240603 : MonoBehaviour
{
    readonly string url = "https://atentsexample.azurewebsites.net/rankdata";
    RankLine[] rankLines;
    Button rankGetButton;

    private void Start()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        Transform parent = transform.parent;
        Transform button = parent.GetChild(1);
        rankGetButton = button.GetComponent<Button>();
        rankGetButton.onClick.AddListener(OnRankGetClick);
    }

    private void OnRankGetClick()
    {
        StartCoroutine(GetRank());
    }

    IEnumerator GetRank()
    {
        // using : C#에서 리소스 관리를 위해 사용된다.
        // 특정 리소스를 사용한 후에 자동으로 정리(Clean-up)
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                RankData data = JsonUtility.FromJson<RankData>(json);
                int min = Mathf.Min(rankLines.Length, data.rankerName.Length, data.highScore.Length);
                for(int i = 0;i< min; i++)
                {
                    rankLines[i].SetRankerName(data.rankerName[i]);
                    rankLines[i].SetHighScore(data.highScore[i]);
                }
                for(int i = min;i<rankLines.Length ;i++)
                {
                    rankLines[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}
