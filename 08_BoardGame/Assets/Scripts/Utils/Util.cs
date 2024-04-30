using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 피셔 예이츠 알고리즘을 통한 셔플 함수
    /// </summary>
    /// <typeparam name="T">셔플할 데이터 타입</typeparam>
    /// <param name="source">셔플할 데이터가 들어있는 배열</param>
    public static void Shuffle<T>(T[] source)
    {
        for(int i=source.Length-1; i>-1; i--)
        {
            int index = Random.Range(0, i);
            (source[index], source[i]) = (source[i], source[index]);
        }
    }
}