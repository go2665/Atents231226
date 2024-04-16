using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSubPanel : MonoBehaviour
{
    RankLine[] rankLines;

    public enum RankType
    {
        Action = 0,
        Time,
    }

    public RankType rankType = RankType.Action;

    private void Start()
    {
        // 적절한 타이밍에 Refresh 함수 실행되게 만들기
    }

    void Refresh()
    {
        // rankLines를 RankDataManager에 있는 데이터를 기반으로 갱신
    }
}
