using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankData<T>
{
    // 리스트에 넣었을 떄 정렬이 되어야 한다.
    // T 타입은 반드시 IComparable을 상속 받은 것만 가능하다.

    readonly T data;
    public T Data => data;

    readonly string name;
    public string Name => name;

    public RankData(T data, string name)
    {
        this.data = data;
        this.name = name;
    }
}

public class RankDataManager : MonoBehaviour
{
    const int RankCount = 10;

    // 랭킹 정보
    List<RankData<int>> actionRank;
    List<RankData<float>> timeRank;


#if UNITY_EDITOR
    public void Test_RankSetting()
    {
        actionRank = new List<RankData<int>>(10);
        actionRank.Add(new(1, "AAA"));
        actionRank.Add(new(10, "BBB"));
        actionRank.Add(new(20, "CCC"));
        actionRank.Add(new(30, "DDD"));
        actionRank.Add(new(40, "EEE"));

        timeRank = new List<RankData<float>>(10);
        timeRank.Add(new(10.0f, "AAA"));
        timeRank.Add(new(20, "BBB"));
        timeRank.Add(new(30, "CCC"));
        timeRank.Add(new(40, "DDD"));
        timeRank.Add(new(50, "EEE"));
    }

    public void Test_ActionUpdate(int rank, string name)
    {
        actionRank.Add(new(rank, name));
        actionRank.Sort();
    }

    public void Test_TimeUpdate(int rank, string name)
    {
        timeRank.Add(new(rank, name));
        timeRank.Sort();
    }
#endif
}
