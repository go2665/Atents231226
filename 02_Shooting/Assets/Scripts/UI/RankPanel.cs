using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 패널에서 표시되는 랭크 한 줄
    /// </summary>
    RankLine[] rankLines;

    /// <summary>
    /// 최고 득점(1등~5등)
    /// </summary>
    int[] highScores;

    /// <summary>
    /// 최고 득점자 이름(1등~5등)
    /// </summary>
    string[] rankerNames;

    /// <summary>
    /// 여기서 표시할 랭크 수
    /// </summary>
    const int rankCount = 5;

    private void Awake()
    {
        rankLines = GetComponentsInChildren<RankLine>();
        highScores = new int[rankCount];
        rankerNames = new string[rankCount];
    }

    /// <summary>
    /// 랭킹 데이터를 초기값으로 모두 설정하는 함수
    /// </summary>
    void SetDefaultData()
    {
        for(int i = 0; i < rankCount; i++)
        {

        }

        // 1st AAA 1000000
        // 2nd BBB 100000
        // 3rd CCC 10000
        // 4th DDD 1000
        // 3th CCC 100
    }

    /// <summary>
    /// 랭킹 데이터를 파일에 저장하는 함수
    /// </summary>
    void SaveRankData()
    {

    }

    /// <summary>
    /// 저장된 랭킹 데이터를 불러오는 함수
    /// </summary>
    /// <returns>성공여부(true면 성공, false면 실패)</returns>
    bool LoadRankData()
    {
        bool result = false;
        return result;
    }

    /// <summary>
    /// 랭킹 데이터를 업데이트하는 함수
    /// </summary>
    /// <param name="score"></param>
    void UpdateRankData(int score)
    {

    }

    /// <summary>
    /// 현재 설정된 랭킹 데이터에 맞게 UI 갱신하는 함수
    /// </summary>
    void RefreshRankLines()
    {
        //rankLines[0].SetData()
    }

    public void Test_DefaultRankPanel()
    {
        SetDefaultData();
        RefreshRankLines();
    }
}
