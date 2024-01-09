using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
{
    /// <summary>
    /// 풀에서 관리할 오브젝트의 프리팹
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// 풀의 크기. 처음에 생성하는 오브젝트의 개수. 모든 개수는 2^n로 잡는 것이 좋다.
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// T타입으로 지정된 오브젝트의 배열. 생성된 모든 오브젝트가 있는 배열.
    /// </summary>
    T[] pool;

    /// <summary>
    /// 현재 사용가능한(비활성화되어있는) 오브젝트들을 관리하는 큐
    /// </summary>
    Queue<T> readyQueue;

    public void Initialize()
    {
        pool = new T[poolSize];                 // 배열의 크기만큼 new
        readyQueue = new Queue<T>(poolSize);    // 레디큐를 만들고 capacity를 poolSize로 지정

        GenerateObjects(0, poolSize, pool);
    }

    /// <summary>
    /// 풀에서 사용할 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="start">새로 생성 시작할 인덱스</param>
    /// <param name="end">새로 생성이 끝나는 인덱스+1</param>
    /// <param name="results">생성된 오브젝트가 들어갈 배열</param>
    void GenerateObjects(int start, int end, T[] results)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();

            results[i] = comp;
            obj.SetActive(false);
        }
    }
}
