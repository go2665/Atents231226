using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Slime = 0,    
}

public class Factory : Singleton<Factory>
{
    /// <summary>
    /// 노이즈 반지름
    /// </summary>
    public float noisePower = 0.5f;

    ItemPool itemPool;
    //SlimePool slimePool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        itemPool = GetComponentInChildren<ItemPool>();
        if(itemPool != null ) itemPool.Initialize();

        //slimePool = GetComponentInChildren<SlimePool>();
        //if( slimePool != null ) slimePool.Initialize();


    }
 
    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            //case PoolObjectType.Slime:
                //result = slimePool.GetObject(position, euler).gameObject;
            //    break;
        }

        return result;
    }

    ///// <summary>
    ///// 슬라임 하나 가져오는 함수
    ///// </summary>
    ///// <returns>배치된 슬라임 하나</returns>
    //public Slime GetSlime()
    //{
    //    return slimePool.GetObject();        
    //}

    ///// <summary>
    ///// 슬라임 하나를 특정 위치에, 특정 각도로 배치
    ///// </summary>
    ///// <param name="position">배치될 위치</param>
    ///// <param name="angle">배치 될 때의 각도</param>
    ///// <returns>배치된 슬라임 하나</returns>
    //public Slime GetSlime(Vector3 position, float angle = 0.0f)
    //{
    //    return slimePool.GetObject(position, angle * Vector3.forward);
    //}

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    public GameObject MakeItem(ItemCode code)
    {
        ItemData data = GameManager.Instance.ItemData[code];    // 아이템 데이터 받아오고
        ItemObject obj = itemPool.GetObject();
        obj.ItemData = data;                    // 풀에서 하나 꺼내고 데이터 설정

        return obj.gameObject;
    }

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">위치에 노이즈를 적용할지 여부(true면 적용, false면 안함)</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    public GameObject MakeItem(ItemCode code, Vector3 position, bool useNoise = false)
    {
        GameObject obj = MakeItem(code);
        Vector3 noise = Vector3.zero;
        if( useNoise )
        {
            Vector2 rand = Random.insideUnitCircle * noisePower;
            noise.x = rand.x;
            noise.z = rand.y;
        }
        obj.transform.position = position + noise;

        return null;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <returns>아이템의 게임 오브젝트들</returns>
    public GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] items = new GameObject[count];
        for(int i = 0;i<count;i++)
        {
            items[i] = MakeItem(code);
        }
        return items;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">위치에 노이즈를 적용할지 여부(true면 적용, false면 안함)</param>
    /// <returns>아이템의 게임 오브젝트들</returns>
    public GameObject[] MakeItems(ItemCode code, uint count, Vector3 position, bool useNoise = false)
    {
        GameObject[] items = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            items[i] = MakeItem(code, position, useNoise);
        }
        return items;
    }
}