using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Slime = 0,    
}

public class Factory : Singleton<Factory>
{
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

    public GameObject MakeItem(ItemCode code)
    {
        ItemData data = GameManager.Instance.ItemData[code];
        ItemObject obj = itemPool.GetObject();
        obj.ItemData = data;

        return obj.gameObject;
    }

    public GameObject MakeItem(ItemCode code, Vector3 position, bool useNoise = false)
    {
        return null;
    }
}